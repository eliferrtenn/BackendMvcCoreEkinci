using Ekinci.CMS.Business.Models.Requests.VideosRequests;
using Ekinci.CMS.Business.Models.Responses.VideosResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IVideosService
    {
        Task<ServiceResult> AddVideo(AddVideosRequest request,IFormFile PhotoUrl);
        Task<ServiceResult> UpdateVideo(UpdateVideosRequest request,IFormFile PhotoUrl);
        Task<ServiceResult> DeleteVideo(DeleteVideosRequest request);
        Task<ServiceResult<List<ListVideosResponses>>> GetAll();
        Task<ServiceResult<GetVideoResponse>> GetVideo(int VideoID);
    }
}