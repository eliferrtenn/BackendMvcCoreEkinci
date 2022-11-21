using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Ekinci.WebAPI.Business.Models.Responses.MemberResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IMemberService
    {
        Task<ServiceResult<GetMemberResponse>> GetMember();
        Task<ServiceResult> UpdateMember(UpdateMemberRequest request);
        Task<ServiceResult> DeleteMember();
    }
}