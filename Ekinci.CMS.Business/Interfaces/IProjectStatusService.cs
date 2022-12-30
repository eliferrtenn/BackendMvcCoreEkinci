using Ekinci.CMS.Business.Models.Requests.ProjectRequests;
using Ekinci.CMS.Business.Models.Requests.ProjectStatusResponses;
using Ekinci.CMS.Business.Models.Responses.ProjectStatusResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IProjectStatusService
    {
        Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll();
        Task<ServiceResult<GetProjectStatusResponse>> GetProjectStatus(int projectStatusID);
        Task<ServiceResult> UpdateProjectStatus(UpdateProjectStatusRequest request, IFormFile PhotoUrl);
    }
}