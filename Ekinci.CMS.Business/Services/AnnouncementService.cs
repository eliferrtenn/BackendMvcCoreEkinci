using Ekinci.CMS.Business.Extensions;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AnnouncementRequests;
using Ekinci.CMS.Business.Models.Responses.AnnouncementResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Ekinci.CMS.Business.Services
{
    public class AnnouncementService : BaseService, IAnnouncementService
    {
        const string fileThumb = "AnnouncementPhoto/Thumb/";
        const string file = "AnnouncementPhoto/General/";
        public AnnouncementService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }
        public async Task<ServiceResult> AddAnnouncement(AddAnnouncementRequest request, IEnumerable<IFormFile> PhotoUrls,IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Announcements.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta haber/duyuru zaten kayıtlıdır.");
                return result;
            }
            var announcement = new Announcement();
            if (PhotoUrl != null)
            {
                Guid guid = Guid.NewGuid();
                var filePaths = new List<string>();
                if (PhotoUrl.Length > 0)
                {
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = fileThumb + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    announcement.ThumbUrl = type;
                }
            }
            announcement.Title = request.Title;
            announcement.Description = request.Description;
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            var id = announcement.ID;
            if (PhotoUrls != null)
            {
                foreach (var photo in PhotoUrls)
                {
                    var announcementPhoto = new AnnouncementPhotos();
                    announcementPhoto.NewsID = id;
                    Guid guid = Guid.NewGuid();
                    var filePaths = new List<string>();
                    if (photo.Length > 0)
                    {
                        var path = Path.GetExtension(photo.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        announcementPhoto.PhotoUrl = type;
                        _context.AnnouncementPhotos.Add(announcementPhoto);
                    }
                }
            }
            await _context.SaveChangesAsync();
            result.SetSuccess("Duyuru-haber başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult<List<ListAnnouncementsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListAnnouncementsResponse>>();
            if (_context.Announcements.Count() == 0)
            {
                result.SetError("Haber/Duyuru bulunamadı");
                return result;
            }
            var announcements = await (from announ in _context.Announcements
                                       where announ.IsEnabled==true
                                       select new ListAnnouncementsResponse
                                       {
                                           ID = announ.ID,
                                           Title = announ.Title,
                                           Description = announ.Description,
                                           ThumbUrl =ekinciUrl+announ.ThumbUrl,
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
                                                          where announphoto.IsEnabled == true
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
                                          ThumbUrl=ekinciUrl+announ.ThumbUrl,
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

        public async Task<ServiceResult> DeleteAnnouncementPhoto(int announcementPhotoID)
        {
            var result = new ServiceResult();
            var history = await _context.AnnouncementPhotos.FirstOrDefaultAsync(x => x.ID == announcementPhotoID);
            if (history == null)
            {
                result.SetError("Duyuru fotoğrafı Bulunamadı!");
                return result;
            }
            history.IsEnabled = false;
            _context.AnnouncementPhotos.Update(history);
            await _context.SaveChangesAsync();
            result.SetSuccess("Fotoğraf silindi.");
            return result;
        }

        public async Task<ServiceResult> UpdateAnnouncement(UpdateAnnouncementRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == true)
            {
                result.SetError("Bu başlıkta haber/duyuru zaten kayıtlıdır.");
                return result;
            }
            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (announcement == null)
            {
                result.SetError("Tarih bilgisi Bulunamadı!");
                return result;
            }
            else
            {
                if (PhotoUrl != null)
                {
                    Guid guid = Guid.NewGuid();
                    var filePaths = new List<string>();
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + announcement.ThumbUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = fileThumb + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        announcement.ThumbUrl = type;
                    }
                }
                announcement.Title = request.Title;
                announcement.Description = request.Description;
                _context.Announcements.Update(announcement);
                await _context.SaveChangesAsync();
                var id = announcement.ID;
                if (PhotoUrls != null)
                {
                    foreach (var photo in PhotoUrls)
                    {
                        var announcementPhoto = new AnnouncementPhotos();
                        announcementPhoto.NewsID = id;
                        Guid guid = Guid.NewGuid();
                        var filePaths = new List<string>();
                        if (photo.Length > 0)
                        {
                            var path = Path.GetExtension(photo.FileName);
                            var type = file + guid.ToString() + path;
                            var filePath = "wwwroot/Dosya/" + type;
                            var filePathBunnyCdn = "/ekinci/" + type;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                            announcementPhoto.PhotoUrl = type;
                            _context.AnnouncementPhotos.Add(announcementPhoto);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                result.SetSuccess("Duyuru-haber başarıyla eklendi!");
                return result;
            }


        }

        public async Task<ServiceResult> DeleteAnnouncement(int announcementID)
        {
            var result = new ServiceResult();
            var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.ID == announcementID);
            if (announcement == null)
            {
                result.SetError("Duyuru Bulunamadı!");
                return result;
            }
            announcement.IsEnabled = false;
            _context.Announcements.Update(announcement);
            await _context.SaveChangesAsync();
            result.SetSuccess("Duyuru silindi.");
            return result;
        }
    }
}