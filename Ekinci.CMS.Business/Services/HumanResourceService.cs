using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.CMS.Business.Models.Responses.HumanResourceResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class HumanResourceService : BaseService, IHumanResourceService
    {
        public HumanResourceService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetHumanResourceResponse>> GetHumanResource()
        {
            var result = new ServiceResult<GetHumanResourceResponse>();
            var humanResource = await (from human in _context.HumanResources
                                       select new GetHumanResourceResponse
                                       {
                                           ID = human.ID,
                                           Title = human.Title,
                                           Description = human.Description,
                                       }).FirstAsync();
            result.Data = humanResource;
            return result;
        }

        public async Task<ServiceResult> UpdateHumanResource(UpdateHumanResourceRequest request)
        {
            var result = new ServiceResult();
            var humanResource = await _context.HumanResources.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (humanResource == null)
            {
                result.SetError("İnsan Kaynakları Bulunamadı!");
                return result;
            }

            humanResource.Title = request.Title;
            humanResource.Description = request.Description;
            _context.HumanResources.Update(humanResource);
            await _context.SaveChangesAsync();
            result.SetSuccess("İnsan Kaynakları başarıyla güncellendi!");
            return result;
        }
    }
}