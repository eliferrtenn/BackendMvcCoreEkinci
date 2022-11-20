using Ekinci.Common.Business;
using Ekinci.Common.JwtModels;
using Ekinci.WebAPI.Business.Models.Requests.AccountRequests;
using Ekinci.WebAPI.Business.Models.Responses.AccountResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<LoginResponse>> Login(LoginRequest request);
        Task<ServiceResult<Token>> LoginSmsVerification(LoginSmsVerificationRequest request);
    }
}