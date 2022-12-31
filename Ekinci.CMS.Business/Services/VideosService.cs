using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.VideosRequests;
using Ekinci.CMS.Business.Models.Responses.VideosResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class VideosService : BaseService, IVideosService
    {
        const string file = "Video/";

        public VideosService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddVideo(AddVideosRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Videos.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            var video = new Videos();
            if(PhotoUrl!= null) {
                var fileUploadResult = _fileUpload.Upload(PhotoUrl, file);
                if (!fileUploadResult.IsSuccess)
                {
                    result.SetError(_localizer["PhotoCouldNotUploaded"]);
                    return result;
                }
                video.PhotoUrl = fileUploadResult.FileName;
            }

            video.Title = request.Title;
            video.Description = request.Description;
            video.VideoUrl = request.VideoUrl;
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteVideo(DeleteVideosRequest request)
        {
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (video == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }

            video.IsEnabled = false;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListVideosResponses>>> GetAll()
        {
            var result = new ServiceResult<List<ListVideosResponses>>();
            if (_context.Videos.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var videos = await (from vid in _context.Videos
                                where vid.IsEnabled == true
                                select new ListVideosResponses
                                {
                                    ID = vid.ID,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    VideoUrl = vid.VideoUrl,
                                    PhotoUrl=vid.PhotoUrl.PrepareCDNUrl(file),
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
                                   VideoUrl = vid.VideoUrl,
                                   PhotoUrl = vid.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            if (video == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = video;
            return result;
        }

        public async Task<ServiceResult> UpdateVideo(UpdateVideosRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (video == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            if(PhotoUrl != null)
            {
                var fileUploadResult = _fileUpload.Upload(PhotoUrl, file);
                if (!fileUploadResult.IsSuccess)
                {
                    result.SetError(_localizer["PhotoCouldNotUploaded"]);
                    return result;
                }
                video.PhotoUrl = fileUploadResult.FileName;
            }
            video.Title = request.Title;
            video.Description = request.Description;
            video.VideoUrl = request.VideoUrl;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordUpdated"]);
            return result;
        }

        public async Task<ServiceResult<UpdateVideosRequest>> UpdateVideo(int VideoID)
        {
            var result = new ServiceResult<UpdateVideosRequest>();
            var video = await(from vid in _context.Videos
                              where vid.ID == VideoID
                              select new UpdateVideosRequest
                              {
                                  ID = vid.ID,
                                  Title = vid.Title,
                                  Description = vid.Description,
                                  VideoUrl = vid.VideoUrl,
                                  PhotoUrl = vid.PhotoUrl.PrepareCDNUrl(file),
                              }).FirstAsync();
            if (video == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = video;
            return result;
        }
    }
}