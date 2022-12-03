using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Ekinci.CMS.Business.Models.Requests.VideosRequests;
using Ekinci.CMS.Business.Models.Responses.PressResponses;
using Ekinci.CMS.Business.Models.Responses.VideosResponses;
using Ekinci.Common.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IVideosService
    {
        Task<ServiceResult> AddVideo(AddVideosRequest request);
        Task<ServiceResult> UpdateVideo(UpdateVideosRequest request);
        Task<ServiceResult> DeleteVideo(DeleteVideosRequest request);
        Task<ServiceResult<List<ListVideosResponses>>> GetAll();
        Task<ServiceResult<GetVideoResponse>> GetVideo(int VideoID);
    }
}
