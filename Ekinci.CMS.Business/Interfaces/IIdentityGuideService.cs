using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.CMS.Business.Models.Responses.IdentityGuideResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IIdentityGuideService
    {
        Task<ServiceResult> UpdateIdentityGuide(UpdateIdentityGuideRequest request);
        Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide();
    }
}