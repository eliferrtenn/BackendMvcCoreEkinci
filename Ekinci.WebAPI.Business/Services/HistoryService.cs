using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.HistoryResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class HistoryService : BaseService, IHistoryService
    {
        public HistoryService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListHistoriesResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListHistoriesResponse>>();
            var histories = await (from hist in _context.Histories
                                   where hist.IsEnabled==true
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

        public async Task<ServiceResult<GetHistoryResponse>> GetHistory(int historyID)
        {
            var result = new ServiceResult<GetHistoryResponse>();
            var histories = await (from hist in _context.Histories
                                   where hist.ID == historyID
                                   select new GetHistoryResponse
                                   {
                                       ID = hist.ID,
                                       Title = hist.Title,
                                       StartDate = hist.StartDate.ToFormattedDate(),
                                       EndDate = hist.EndDate.ToFormattedDate(),
                                       PhotoUrl = ekinciUrl + hist.PhotoUrl,
                                   }).FirstAsync();
            if (histories == null)
            {
                result.SetError("Tarihçe bulunamadı");
                return result;
            }
            result.Data = histories;
            return result;
        }
    }
}