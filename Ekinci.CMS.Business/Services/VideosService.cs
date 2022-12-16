using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.VideosRequests;
using Ekinci.CMS.Business.Models.Responses.VideosResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Ekinci.CMS.Business.Services
{
    public class VideosService : BaseService, IVideosService
    {
        const string file = "Video/";
        public VideosService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddVideo(AddVideosRequest request,IFormFile VideoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Videos.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu isimde video zaten kayıtlıdır.");
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var video = new Videos();
            if (VideoUrl != null)
            {
                if (VideoUrl.Length > 0)
                {
                    var path = Path.GetExtension(VideoUrl.FileName);
                    var type = file + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    video.VideoUrl = type;
                }
            }
            video.Title = request.Title;
            video.Description = request.Description;
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            result.SetSuccess("Video başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> DeleteVideo(DeleteVideosRequest request)
        {
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (video == null)
            {
                result.SetError("Video Bulunamadı!");
                return result;
            }

            video.IsEnabled = false;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
            result.SetSuccess("Video silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListVideosResponses>>> GetAll()
        {
            var result = new ServiceResult<List<ListVideosResponses>>();
            if (_context.Videos.Count() == 0)
            {
                result.SetError("Videolar bulunamadı");
                return result;
            }
            var videos = await (from vid in _context.Videos
                                where vid.IsEnabled==true
                                select new ListVideosResponses
                                {
                                    ID = vid.ID,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    VideoUrl = ekinciUrl+vid.VideoUrl,
                                }).ToListAsync();
            result.Data = videos;
            return result;
        }

        public async Task<ServiceResult<GetVideoResponse>> GetVideo(int VideoID)
        {
            var result = new ServiceResult<GetVideoResponse>();
            var video = await (from vid in _context.Videos
                               where vid.ID == VideoID
                               select new GetVideoResponse
                               {
                                   ID = vid.ID,
                                   Title = vid.Title,
                                   Description = vid.Description,
                                   VideoUrl =ekinciUrl+ vid.VideoUrl,
                               }).FirstAsync();
            if (video == null)
            {
                result.SetError("Video bulunamadı");
                return result;
            }
            result.Data = video;
            return result;
        }

        public async Task<ServiceResult> UpdateVideo(UpdateVideosRequest request,IFormFile VideoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (video == null)
            {
                result.SetError("Video Bulunamadı!");
                return result;
            }
            if (VideoUrl != null)
            {
                if (VideoUrl.Length > 0)
                {
                    await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + video.VideoUrl);
                    var path = Path.GetExtension(VideoUrl.FileName);
                    var type = file + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    video.VideoUrl = type;
                }
            }
            else
            {
                video.VideoUrl = request.VideoUrl;
            }
            video.Title = request.Title;
            video.Description = request.Description;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();

            result.SetSuccess("Video başarıyla güncellendi!");
            return result;
        }
    }
}