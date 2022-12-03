using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.CMS.Business.Models.Responses.SustainabilityResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ISustainabilityService
    {
        Task<ServiceResult> UpdateSustainability(UpdateSustainabilityRequest request);
        Task<ServiceResult<GetSustainabilityResponse>> GetSustainability();
    }
}
