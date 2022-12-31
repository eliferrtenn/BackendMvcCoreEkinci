using Ekinci.CMS.Business.Models.Requests.AnnouncementRequests;
using Ekinci.CMS.Business.Models.Responses.AnnouncementResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IAnnouncementService
    {
        Task<ServiceResult> AddAnnouncement(AddAnnouncementRequest request,IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl);
        Task<ServiceResult<UpdateAnnouncementRequest>> UpdateAnnouncement(int announcementID);
        Task<ServiceResult> UpdateAnnouncement(UpdateAnnouncementRequest request,IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl);
        Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll();
        Task<ServiceResult<GetAnnouncementResponse>> GetAnnouncement(int announcementID);
        Task<ServiceResult> DeleteAnnouncementPhoto(int announcementPhotoID);
        Task<ServiceResult> DeleteAnnouncement(int announcementID);
    }
}