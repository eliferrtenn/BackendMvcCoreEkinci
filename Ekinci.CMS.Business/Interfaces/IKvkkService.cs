using Ekinci.CMS.Business.Models.Requests.KvkkRequests;
using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.CMS.Business.Models.Responses.KvkkResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IKvkkService
    {
        Task<ServiceResult> AddKvkk(AddKvkkRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> UpdateKvkk(UpdateKvkkResponse request, IFormFile PhotoUrl);
        Task<ServiceResult<GetKvkkResponse>> GetKvkk();
    }
}