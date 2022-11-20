using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IMemberService
    {
        Task<ServiceResult<GetAnnouncementResponse>> GetMember();
        Task<ServiceResult> UpdateMember(UpdateMemberRequest request);
        Task<ServiceResult> DeleteMember();
    }
}