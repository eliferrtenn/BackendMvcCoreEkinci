using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.SustainabilityResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface ISustainabilityService
    {
        Task<ServiceResult<List<ListSustainabilitiesResponse>>> GetAll();
        Task<ServiceResult<GetSustainabilityResponse>> GetSustainability(int sustainabilityID);
    }
}