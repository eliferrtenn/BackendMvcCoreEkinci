using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.PressResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IPressService
    {
        Task<ServiceResult<List<ListPressesResponse>>> GetAll();
        Task<ServiceResult<GetPressResponse>> GetPress(int pressID);
    }
}