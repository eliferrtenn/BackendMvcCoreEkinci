using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.CommercialAreaResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface ICommercialAreaService
    {
        Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll();
        Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID);
    }
}