using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.VideosResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class VideosService : BaseService, IVideosService
    {
        public VideosService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
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
                                    PhotoUrl = vid.PhotoUrl,
                                    //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                }).ToListAsync();
            result.Data = videos;
            return result;
        }

        public async Task<ServiceResult<GetVideoResponse>> GetVideo(int videoID)
        {
            var result = new ServiceResult<GetVideoResponse>();
            var videos = await (from vid in _context.Videos
                                where vid.ID == videoID
                                select new GetVideoResponse
                                {
                                    ID = vid.ID,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    PhotoUrl = vid.PhotoUrl,
                                    //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                }).FirstAsync();
            if (videos == null)
            {
                result.SetError("Video bulunamadı");
                return result;
            }
            result.Data = videos;
            return result;
        }
    }
}