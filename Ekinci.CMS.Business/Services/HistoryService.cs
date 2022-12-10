using BunnyCDN.Net.Storage;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Responses.HistoryResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.IO;

namespace Ekinci.CMS.Business.Services
{
    public class HistoryService : BaseService, IHistoryService
    {
        BunnyCDNStorage bunnyCDNStorage = new BunnyCDNStorage("ekinci", "257e5f3c-55fc-40b8-b00f2a941162-b427-4e2d", "de");

        public HistoryService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }
        public async Task<ServiceResult> AddHistory(AddHistoryRequest request,IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Histories.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta tarihçe zaten kayıtlıdır.");
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var history = new History();
            if (PhotoUrl != null)
            {
                if (PhotoUrl.Length > 0)
                {
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/History/" + type;
                    var filePathBunnyCdn = "/ekinci/History/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    history.PhotoUrl = "History/"+type;
                }
            }
            history.Title = request.Title;
            history.StartDate = request.StartDate;
            history.EndDate = request.EndDate;
            //TODO:photourl
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            result.SetSuccess("Tarihçe başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> DeleteHistory(DeleteHistoryRequest request)
        {
            var result = new ServiceResult();
            var history = await _context.Histories.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (history == null)
            {
                result.SetError("Tarih bilgisi Bulunamadı!");
                return result;
            }
            history.IsEnabled = false;
            _context.Histories.Update(history);
            await _context.SaveChangesAsync();

            result.SetSuccess("Tarih bilgisi silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListHistoriesResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListHistoriesResponse>>();
            var histories = await (from hist in _context.Histories
                                   select new ListHistoriesResponse
                                   {
                                       ID = hist.ID,
                                       Title = hist.Title,
                                       StartDate = hist.StartDate.ToFormattedDate(),
                                       EndDate = hist.EndDate.ToFormattedDate(),
                                       PhotoUrl = "https://ekinci.b-cdn.net/"+hist.PhotoUrl,
                                       //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                   }).ToListAsync();
            result.Data = histories;
            return result;
        }

        public async Task<ServiceResult<GetHistoryResponse>> GetHistory(int HistoryID)
        {
            var result = new ServiceResult<GetHistoryResponse>();
            var histories = await (from hist in _context.Histories
                                   where hist.ID == HistoryID
                                   select new GetHistoryResponse
                                   {
                                       ID = hist.ID,
                                       Title = hist.Title,
                                       StartDate = hist.StartDate,
                                       EndDate = hist.EndDate,
                                       PhotoUrl = "https://ekinci.b-cdn.net/" + hist.PhotoUrl,
                                       //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                   }).FirstAsync();
            if (histories == null)
            {
                result.SetError("Tarihçe bulunamadı");
                return result;
            }
            result.Data = histories;
            return result;
        }

        public async Task<ServiceResult> UpdateHistory(UpdateHistoryRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var history = await _context.Histories.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (history == null)
            {
                result.SetError("Tarih bilgisi Bulunamadı!");
                return result;
            }
            if (PhotoUrl != null)
            {
                if (PhotoUrl.Length > 0)
                {
                    await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + history.PhotoUrl);
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/History/" + type;
                    var filePathBunnyCdn = "/ekinci/History/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    history.PhotoUrl = "History/"+type;
                }

            }
            history.Title = request.Title;
            history.StartDate = request.StartDate;
            history.EndDate = request.EndDate;
            //TODO:photourl
            _context.Histories.Update(history);
            await _context.SaveChangesAsync();

            result.SetSuccess("Tarihçe  başarıyla güncellendi!");
            return result;
        }
    }
}