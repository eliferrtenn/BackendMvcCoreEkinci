using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Ekinci.WebAPI.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.WebAPI.Business.Models.Responses.MemberResponse;
using Ekinci.WebAPI.Business.Models.Responses.TechnicalServiceDemandResponses;
using Microsoft.AspNetCore.Http;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface ITechnicalServiceDemandService
    {
        Task<ServiceResult<List<ListTechnicalServiceDemandResponse>>> GetAll();
        Task<ServiceResult<GetTechnicalServiceDemandResponse>> GetTechnicalServiceDemand(int technicalDemandServiceID);
        Task<ServiceResult> AddTechnicalServiceDemand(AddTechnicalServiceDemandRequest request);
        Task<ServiceResult> EditTechnicalServiceDemand(EditTechnicalServiceDemandRequest request);
        Task<ServiceResult> DeleteTechnicalServiceDemand(DeleteTechnicalServiceDemandRequest request);
    }
}
