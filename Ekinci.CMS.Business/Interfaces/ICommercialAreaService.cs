using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.CMS.Business.Models.Responses.CommercialAreaResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ICommercialAreaService
    {
        Task<ServiceResult> AddCommercialArea(AddCommercialAreaRequest request, IFormFile PhotoUrl);
        Task<ServiceResult<UpdateCommercialAreaRequest>> UpdateCommercialArea(int CommercialAreaID);
        Task<ServiceResult> UpdateCommercialArea(UpdateCommercialAreaRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> DeleteCommercialArea(DeleteCommercialAreaRequest request);
        Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll();
        Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID);
    }
}