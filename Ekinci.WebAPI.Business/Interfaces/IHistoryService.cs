using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.HistoryResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IHistoryService
    {
        Task<ServiceResult<List<ListHistoriesResponse>>> GetAll();
        Task<ServiceResult<GetHistoryResponse>> GetHistory(int historyID);
    }
}