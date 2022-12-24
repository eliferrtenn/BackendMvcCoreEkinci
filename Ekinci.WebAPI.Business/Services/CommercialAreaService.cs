using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.CommercialAreaResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class CommercialAreaService : BaseService, ICommercialAreaService
    {
        public CommercialAreaService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListCommercialAreasResponse>>();
            var commercials = await (from commercial in _context.CommercialAreas
                                     select new ListCommercialAreasResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = ekinciUrl + commercial.PhotoUrl,
                                     }).ToListAsync();
            result.Data = commercials;
            return result; ;
        }

        public async Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID)
        {
            var result = new ServiceResult<GetCommercialAreaResponse>();

            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.ID == CommercialAreaID
                                     select new GetCommercialAreaResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = ekinciUrl + commercial.PhotoUrl,
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError("Ticari alan bulunamadı");
                return result;
            }
            result.Data = commercials;
            return result;
        }
    }
}