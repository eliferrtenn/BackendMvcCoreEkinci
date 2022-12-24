using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Responses.HistoryResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class HistoryService : BaseService, IHistoryService
    {
        const string file = "History/";

        public HistoryService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult> AddHistory(AddHistoryRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Histories.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["HistoryWithNameAlreadyExist"]);
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
                    var type = file + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    history.PhotoUrl = type;
                }
            }
            history.Title = request.Title;
            history.StartDate = request.StartDate;
            history.EndDate = request.EndDate;
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["HistoryAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteHistory(DeleteHistoryRequest request)
        {
            var result = new ServiceResult();
            var history = await _context.Histories.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (history == null)
            {
                result.SetError(_localizer["HistoryNotFound"]);
                return result;
            }
            history.IsEnabled = false;
            _context.Histories.Update(history);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["HistoryDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListHistoriesResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListHistoriesResponse>>();
            if (_context.Histories.Count() == 0)
            {
                result.SetError(_localizer["HistoryNotFound"]);
                return result;
            }
            var histories = await (from hist in _context.Histories
                                   where hist.IsEnabled == true
                                   select new ListHistoriesResponse
                                   {
                                       ID = hist.ID,
                                       Title = hist.Title,
                                       StartDate = hist.StartDate.ToFormattedDate(),
                                       EndDate = hist.EndDate.ToFormattedDate(),
                                       PhotoUrl = ekinciUrl + hist.PhotoUrl,
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
                                       PhotoUrl = ekinciUrl + hist.PhotoUrl,
                                   }).FirstAsync();
            if (histories == null)
            {
                result.SetError(_localizer["HistoryNotFound"]);
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
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var history = await _context.Histories.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (history == null)
                {
                    result.SetError(_localizer["HistoryNotFound"]);
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + history.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        history.PhotoUrl = type;
                    }
                }
                history.Title = request.Title;
                history.StartDate = request.StartDate;
                history.EndDate = request.EndDate;
                _context.Histories.Update(history);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["HistoryUpdated"]);
            }
            else
            {
                result.SetError(_localizer["HistoryWithNameAlreadyExist"]);
            }
            return result;
        }
    }
}