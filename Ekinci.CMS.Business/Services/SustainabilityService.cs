using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.CMS.Business.Models.Responses.SustainabilityResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class SustainabilityService : BaseService, ISustainabilityService
    {
        public SustainabilityService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetSustainabilityResponse>> GetSustainability()
        {
            var result = new ServiceResult<GetSustainabilityResponse>();
            var sustainability = await(from sustain in _context.Sustainabilities
                                       select new GetSustainabilityResponse
                                       {
                                           ID = sustain.ID,
                                           Title = sustain.Title,
                                           Description = sustain.Description,
                                           PhotoUrl = sustain.PhotoUrl,
                                           //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                       }).FirstAsync();
            result.Data = sustainability;
            return result;
        }

        public async Task<ServiceResult> UpdateSustainability(UpdateSustainabilityRequest request)
        {
            var result = new ServiceResult();
            var sustainability = await _context.Sustainabilities.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (sustainability == null)
            {
                result.SetError("Sürdürülebilirlik Bulunamadı!");
                return result;
            }

            sustainability.Title = request.Title;
            sustainability.Description = request.Description;
            //TODO:Sürdürülebilirlikte güncelleme işlemi yapılacak
            _context.Sustainabilities.Update(sustainability);
            await _context.SaveChangesAsync();
            result.SetSuccess("Sürdürülebilirlik başarıyla güncellendi!");
            return result;
        }
    }
}