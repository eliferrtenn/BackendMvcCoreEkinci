using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Responses.HistoryResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IHistoryService
    {
        Task<ServiceResult> AddHistory(AddHistoryRequest request);
        Task<ServiceResult> UpdateHistory(UpdateHistoryRequest request);
        Task<ServiceResult> DeleteHistory(DeleteHistoryRequest request);
        Task<ServiceResult<List<ListHistoriesResponse>>> GetAll();
        Task<ServiceResult<GetHistoryResponse>> GetHistory(int HistoryID);
    }
}