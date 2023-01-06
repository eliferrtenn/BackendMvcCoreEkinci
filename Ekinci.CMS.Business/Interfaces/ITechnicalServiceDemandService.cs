using Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceDemandResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ITechnicalServiceDemandService
    {
        Task<ServiceResult<List<ListTechnicalServiceDemandResponse>>> GetAll();
        Task<ServiceResult<GetTechnicalServiceDemandResponse>> GetTechnicalServiceDemand(int technicalDemandServiceID);
        Task<ServiceResult<List<ListNonAssignmentTechnicalServiceDemendResponse>>> ListNonAssignmentTechnicalServiceDemend();
        Task<ServiceResult<List<ListAssignTechnicalServiceDemandResponse>>> ListAssignTechnicalServiceDemand();
        Task<ServiceResult> AssignPersonelTechnicalServiceDemand(AssignPersonelTechnicalServiceDemandRequest request);
        Task<ServiceResult<AssignPersonelTechnicalServiceDemandRequest>> AssignPersonelTechnicalServiceDemand(int technicalDemandServiceID);
    }
}