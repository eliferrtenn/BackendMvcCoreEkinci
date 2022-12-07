using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Responses.AnnouncementResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class AnnouncementService : BaseService, IAnnouncementService
    {
        public AnnouncementService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }
        public async Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListAnnouncementsResponse>>();
            var announcements = await (from announ in _context.Announcements
                                       select new ListAnnouncementsResponse
                                       {
                                           ID = announ.ID,
                                           Title = announ.Title,
                                           Description = announ.Description,
                                           ThumbUrl = announ.ThumbUrl,
                                       }).ToListAsync();

            result.Data = announcements;
            return result;
        }

        public async Task<ServiceResult<GetAnnouncementResponse>> GetAnnouncement(int announcementID)
        {
            var result = new ServiceResult<GetAnnouncementResponse>();

            var announcement = await (from announ in _context.Announcements
                                      where announ.ID == announcementID
                                      let announPhotos = (from announphoto in _context.AnnouncementPhotos
                                                          where announphoto.NewsID == announ.ID
                                                          select new AnnouncementResponse
                                                          {
                                                              ID = announphoto.ID,
                                                              PhotoUrl = announphoto.PhotoUrl
                                                          }).ToList()
                                      select new GetAnnouncementResponse
                                      {
                                          ID = announ.ID,
                                          Title = announ.Title,
                                          Description = announ.Description,
                                          AnnouncementPhotos = announPhotos
                                      }).FirstAsync();
            if (announcement == null)
            {
                result.SetError("Duyuru bulunamadı");
                return result;
            }
            result.Data = announcement;
            return result;
        }
    }
}