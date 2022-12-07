using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.CMS.Business.Models.Responses.IdentityGuideResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class IdentityGuideService : BaseService, IIdentityGuideService
    {
        public IdentityGuideService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide()
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var IdentityGuide = await (from identity in _context.IdentityGuides
                                       select new GetIdentityGuideResponse
                                       {
                                           ID = identity.ID,
                                           Title = identity.Title,
                                           PhotoUrl = identity.PhotoUrl,
                                           //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                       }).FirstAsync();
            result.Data = IdentityGuide;
            return result;
        }

        public async Task<ServiceResult> UpdateIdentityGuide(UpdateIdentityGuideRequest request)
        {
            var result = new ServiceResult();
            var identityGuide = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (identityGuide == null)
            {
                result.SetError("Kurumsal Kimlik Bulunamadı!");
                return result;
            }

            identityGuide.Title = request.Title;
            //TODO:Kurumsal kimlik fotoğraf ve dosya güncelleme işlemi yapılacak
            _context.IdentityGuides.Update(identityGuide);
            await _context.SaveChangesAsync();
            result.SetSuccess("Kurumsal Kimlik başarıyla güncellendi!");
            return result;
        }
    }
}