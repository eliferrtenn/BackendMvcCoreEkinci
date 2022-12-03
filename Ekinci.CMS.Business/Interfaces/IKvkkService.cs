using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.CMS.Business.Models.Responses.KvkkResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IKvkkService
    {
        Task<ServiceResult> UpdateKvkk(UpdateKvkkResponse request);
        Task<ServiceResult<GetKvkkResponse>> GetKvkk();
    }
}