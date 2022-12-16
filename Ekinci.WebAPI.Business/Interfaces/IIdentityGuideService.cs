using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IIdentityGuideService
    {
        Task<ServiceResult<List<ListIdentityGuideResponse>>> GetAll();
        Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int identityID);
    }
}
