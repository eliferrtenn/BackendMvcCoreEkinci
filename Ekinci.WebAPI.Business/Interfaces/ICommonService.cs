using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.BlogResponses;
using Ekinci.WebAPI.Business.Models.Responses.HumanResourceResponse;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse;
using Ekinci.WebAPI.Business.Models.Responses.IntroResponse;
using Ekinci.WebAPI.Business.Models.Responses.KvkkResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface ICommonService
    {        
        Task<ServiceResult<GetKvkkResponse>> GetKvkk();
        Task<ServiceResult<GetIntroResponse>> GetIntro();
        Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide();
        Task<ServiceResult<GetHumanResourceResponse>> GetHumanResorce();
        Task<ServiceResult<GetBlogResponse>> GetBlog();
    }
}