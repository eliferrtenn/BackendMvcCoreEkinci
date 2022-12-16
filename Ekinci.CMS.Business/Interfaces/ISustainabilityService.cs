using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.CMS.Business.Models.Responses.SustainabilityResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ISustainabilityService
    {
        Task<ServiceResult> AddSustainability(AddSustainabilityRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> UpdateSustainability(UpdateSustainabilityRequest request, IFormFile PhotoUr);
        Task<ServiceResult<GetSustainabilityResponse>> GetSustainability();
    }
}