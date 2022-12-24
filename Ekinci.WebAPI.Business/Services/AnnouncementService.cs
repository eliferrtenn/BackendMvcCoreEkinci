using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses;
using Ekinci.WebAPI.Business.Models.Responses.ProjectResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class AnnouncementService : BaseService, IAnnouncementService
    {
        public AnnouncementService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListAnnouncementsResponse>>();
            var announcements = await (from announ in _context.Announcements
                                       where announ.IsEnabled==true
                                       select new ListAnnouncementsResponse
                                       {
                                           ID = announ.ID,
                                           Title = announ.Title,
                                           Description = announ.Description,
                                           AnnouncementDate=announ.AnnouncementDate.ToFormattedDate(),
                                           ThumbUrl=ekinciUrl+announ.ThumbUrl,
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
                                                               PhotoUrl = ekinciUrl + announphoto.PhotoUrl
                                                           }).ToList()
                                      select new GetAnnouncementResponse
                                      {
                                          ID = announ.ID,
                                          Title = announ.Title,
                                          Description = announ.Description,
                                          AnnouncementDate = announ.AnnouncementDate.ToFormattedDate(),
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