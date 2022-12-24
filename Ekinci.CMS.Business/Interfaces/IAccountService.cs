using Ekinci.CMS.Business.Models.Requests.AccountRequests;
using Ekinci.CMS.Business.Models.Responses.AccountResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult> SignIn(LoginRequest request);
        Task<ServiceResult> SignOut();
        Task<ServiceResult<UpdateProfileRequest>> GetProfile();
        Task<ServiceResult> UpdateProfile(UpdateProfileRequest request,IFormFile ProfilePhotoUrl);
        Task<ServiceResult> ForgotPassword(ForgotPasswordRequest request);
    }
}