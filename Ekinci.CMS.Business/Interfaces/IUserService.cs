using Ekinci.CMS.Business.Models.Requests.UserRequests;
using Ekinci.CMS.Business.Models.Responses.UserResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<GetUserResponse>> GetUser();
        Task<ServiceResult> UpdateUser(UpdateUserRequest request);
    }
}