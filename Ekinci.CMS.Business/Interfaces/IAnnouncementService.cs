using Ekinci.CMS.Business.Models.Responses.AnnouncementResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IAnnouncementService
    {
        Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll();
        Task<ServiceResult<GetAnnouncementResponse>> GetAnnouncement(int announcementID);
    }
}