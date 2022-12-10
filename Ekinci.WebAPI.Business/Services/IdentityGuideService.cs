using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class IdentityGuideService : BaseService, IIdentityGuideService
    {
        public IdentityGuideService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
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
                                        PhotoUrl = identity.PhotoUrl,
                                        FileUrl = identity.FileUrl,
                                        //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                    }).ToListAsync();
            result.Data = identities;
            return result;
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int blogID)
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var identities = await (from identity in _context.IdentityGuides
                                    where identity.ID == blogID
                                    select new GetIdentityGuideResponse
                                    {
                                        ID = identity.ID,
                                        Title = identity.Title,
                                        PhotoUrl = identity.PhotoUrl,
                                        FileUrl = identity.FileUrl,
                                        //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
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