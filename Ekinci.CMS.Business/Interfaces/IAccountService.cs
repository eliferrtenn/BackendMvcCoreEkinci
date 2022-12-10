using Ekinci.CMS.Business.Models.Requests.AccountRequests;
using Ekinci.CMS.Business.Models.Responses.AccountResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<LoginResponse>> Login(LoginRequest request);
    }
}