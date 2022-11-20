using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IAnnouncementService
    {
        Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll();
        Task<ServiceResult<GetAnnouncementResponse>> GetAnnouncement(int announcementID);
    }
}