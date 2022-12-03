using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.CMS.Business.Models.Responses.CommercialAreaResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ICommercialAreaService
    {
        Task<ServiceResult> AddCommercialArea(AddCommercialAreaRequest request);
        Task<ServiceResult> UpdateCommercialArea(UpdateCommercialAreaRequest request);
        Task<ServiceResult> DeleteCommercialArea(DeleteCommercialAreaRequest request);
        Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll();
        Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID);
    }
}