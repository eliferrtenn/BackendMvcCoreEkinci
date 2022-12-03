using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.CMS.Business.Models.Responses.HumanResourceResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IHumanResourceService
    {
        Task<ServiceResult> UpdateHumanResource(UpdateHumanResourceRequest request);
        Task<ServiceResult<GetHumanResourceResponse>> GetHumanResource();
    }
}