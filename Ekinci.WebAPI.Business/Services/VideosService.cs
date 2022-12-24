using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.VideosResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class VideosService : BaseService, IVideosService
    {
        public VideosService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListVideosResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListVideosResponse>>();
            var videos = await (from vid in _context.Videos
                                select new ListVideosResponse
                                {
                                    ID = vid.ID,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    PhotoUrl = ekinciUrl + vid.VideoUrl,
                                }).ToListAsync();
            result.Data = videos;
            return result;
        }

        public async Task<ServiceResult<GetVideoResponse>> GetVideo(int videoID)
        {
            var result = new ServiceResult<GetVideoResponse>();
            var video = await (from vid in _context.Videos
                                where vid.ID == videoID
                                select new GetVideoResponse
                                {
                                    ID = vid.ID,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    VideoUrl = ekinciUrl + vid.VideoUrl,
                                }).FirstAsync();
            if (video == null)
            {
                result.SetError("Video bulunamadı");
                return result;
            }
            result.Data = video;
            return result;
        }
    }
}