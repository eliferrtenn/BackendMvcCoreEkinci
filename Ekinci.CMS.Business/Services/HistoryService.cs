using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Responses.HistoryResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class HistoryService : BaseService, IHistoryService
    {
        public HistoryService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddHistory(AddHistoryRequest request)
        {
            var result = new ServiceResult();
            var history = await _context.Histories.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (history != null)
            {
                result.SetError("Bu başlıkta tarihçe zaten kayıtlıdır.");
                return result;
            }
            history!.Title = request.Title;
            history!.StartDate = request.StartDate;
            history!.EndDate = request.EndDate;
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
            history.IsEnabled = true;
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
                                       PhotoUrl = hist.PhotoUrl,
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
                                       StartDate = hist.StartDate.ToFormattedDate(),
                                       EndDate = hist.EndDate.ToFormattedDate(),
                                       PhotoUrl = hist.PhotoUrl,
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

        public async Task<ServiceResult> UpdateHistory(UpdateHistoryRequest request)
        {
            var result = new ServiceResult();
            var history = await _context.Histories.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (history == null)
            {
                result.SetError("Tarih bilgisi Bulunamadı!");
                return result;
            }
            history!.Title = request.Title;
            history!.StartDate = request.StartDate;
            history!.EndDate = request.EndDate;
            //TODO:photourl
            _context.Histories.Update(history);
            await _context.SaveChangesAsync();

            result.SetSuccess("Tarihçe  başarıyla güncellendi!");
            return result;
        }
    }
}