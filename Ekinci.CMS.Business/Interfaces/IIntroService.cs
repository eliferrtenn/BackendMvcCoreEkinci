using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IIntroService
    {
        Task<ServiceResult> AddIntro(AddIntroRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> UpdateIntro(UpdateIntroRequest request, IFormFile PhotoUrl);
        Task<ServiceResult<GetIntroResponse>> GetIntro();
    }
}