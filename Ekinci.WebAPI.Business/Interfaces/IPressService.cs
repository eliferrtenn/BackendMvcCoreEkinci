using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses;
using Ekinci.WebAPI.Business.Models.Responses.PressResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IPressService
    {
        Task<ServiceResult<List<ListPressResponse>>> GetAll();
        Task<ServiceResult<GetPressResponse>> GetPress(int pressID);
    }
}