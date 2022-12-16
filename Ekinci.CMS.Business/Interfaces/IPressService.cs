using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Ekinci.CMS.Business.Models.Responses.PressResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IPressService
    {
        Task<ServiceResult> AddPress(AddPressRequest request,IFormFile PhotoUrl);
        Task<ServiceResult> UpdatePress(UpdatePressRequest request,IFormFile PhotoUrl);
        Task<ServiceResult> DeletePress(DeletePressRequest request);
        Task<ServiceResult<List<ListPressesResponse>>> GetAll();
        Task<ServiceResult<GetPressResponse>> GetPress(int PressID);
    }
}