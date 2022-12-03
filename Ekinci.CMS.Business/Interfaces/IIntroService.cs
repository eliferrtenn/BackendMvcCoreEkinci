using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IIntroService
    {
        Task<ServiceResult> UpdateIntro(UpdateIntroRequest request);
        Task<ServiceResult<GetIntroResponse>> GetIntro();
    }
}