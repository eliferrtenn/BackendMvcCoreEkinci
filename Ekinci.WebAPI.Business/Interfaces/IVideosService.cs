using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.VideosResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IVideosService
    {
        Task<ServiceResult<List<ListVideosResponse>>> GetAll();
        Task<ServiceResult<GetVideoResponse>> GetVideo(int videoID);
    }
}