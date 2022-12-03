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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.CMS.Business.Services
{
    public class VideosService : BaseService, IVideosService
    {
        public VideosService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddVideo(AddVideosRequest request)
        {
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (video != null)
            {
                result.SetError("Bu isimde video zaten kayıtlıdır.");
                return result;
            }
            video!.Title = request.Title;
            video!.Description = request.Description;
            //TODO:videoya url ekleme
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

            video.IsEnabled = true;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
            result.SetSuccess("Video silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListVideosResponses>>> GetAll()
        {
            var result = new ServiceResult<List<ListVideosResponses>>();
            var videos = await(from vid in _context.Videos
                               select new ListVideosResponses
                               {
                                   ID = vid.ID,
                                   Title = vid.Title,
                                   Description = vid.Description,
                                   VideoUrl = vid.VideoUrl,
                                   //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
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
                                   //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                               }).FirstAsync();
            if (video == null)
            {
                result.SetError("Video bulunamadı");
                return result;
            }
            result.Data = video;
            return result;
        }

        public async Task<ServiceResult> UpdateVideo(UpdateVideosRequest request)
        {
            var result = new ServiceResult();
            var video = await _context.Videos.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (video == null)
            {
                result.SetError("Video Bulunamadı!");
                return result;
            }
            video.Title = request.Title;
            video.Description = request.Description;
            //TODO:video url ekleme
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();

            result.SetSuccess("Video başarıyla güncellendi!");
            return result;
        }
    }
}
