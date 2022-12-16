using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.CMS.Business.Models.Responses.IdentityGuideResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IIdentityGuideService
    {
        Task<ServiceResult> AddIdentityGuide(AddIdentityGuideRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> UpdateIdentityGuide(UpdateIdentityGuideRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> DeleteIdentityGuide(DeleteIdentityGuideRequest request);
        Task<ServiceResult<List<ListIdentityGuideResponse>>> GetAll();
        Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int IdentityGuideID);
    }
}