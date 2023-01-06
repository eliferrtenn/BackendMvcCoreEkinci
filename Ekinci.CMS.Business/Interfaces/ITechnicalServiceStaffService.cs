using Ekinci.CMS.Business.Models.Requests.TechnicalServiceStaffRequests;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceStaffResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ITechnicalServiceStaffService
    {
        Task<ServiceResult> AddTechnicalServiceStaff(AddTechnicalServiceStaffRequest request);
        Task<ServiceResult<UpdateTechnicalServiceStaffRequest>> UpdateTechnicalServiceStaff(int TechnicalServiceStaffID);
        Task<ServiceResult> UpdateTechnicalServiceStaff(UpdateTechnicalServiceStaffRequest request);
        Task<ServiceResult> DeleteTechnicalServiceStaff(DeleteTechnicalServiceStaffRequest request);
        Task<ServiceResult<List<ListTechnicalServiceStaffsResponse>>> GetAll();
        Task<ServiceResult<GetTechnicalServiceStaffResponse>> GetTechnicalServiceStaff(int TechnicalServiceStaffID);
    }
}