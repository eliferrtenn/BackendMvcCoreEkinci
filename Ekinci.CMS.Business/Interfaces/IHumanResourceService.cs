using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.CMS.Business.Models.Responses.HumanResourceResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IHumanResourceService
    {
        Task<ServiceResult> AddHumanResource(AddHumanResourceRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> UpdateHumanResource(UpdateHumanResourceRequest request, IFormFile PhotoUrl);
        Task<ServiceResult<GetHumanResourceResponse>> GetHumanResource();
    }
}