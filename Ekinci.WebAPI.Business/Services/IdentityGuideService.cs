using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class IdentityGuideService : BaseService, IIdentityGuideService
    {
        public IdentityGuideService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListIdentityGuideResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListIdentityGuideResponse>>();
            var identities = await (from identity in _context.IdentityGuides
                                    select new ListIdentityGuideResponse
                                    {
                                        ID = identity.ID,
                                        Title = identity.Title,
                                        PhotoUrl = ekinciUrl + identity.PhotoUrl,
                                    }).ToListAsync();
            result.Data = identities;
            return result;
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int identityID)
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var identities = await (from identity in _context.IdentityGuides
                                    where identity.ID == identityID
                                    select new GetIdentityGuideResponse
                                    {
                                        ID = identity.ID,
                                        Title = identity.Title,
                                        PhotoUrl = ekinciUrl + identity.PhotoUrl,
                                    }).FirstAsync();
            if (identities == null)
            {
                result.SetError("Kurumsal kimlik rehberi bulunamadı");
                return result;
            }
            result.Data = identities;
            return result;
        }
    }
}