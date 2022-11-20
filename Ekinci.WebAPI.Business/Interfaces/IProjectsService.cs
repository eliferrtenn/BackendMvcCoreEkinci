using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.ProjectResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IProjectsService
    {
        Task<ServiceResult<List<ListProjectsResponse>>> GetAll();
        Task<ServiceResult<GetProjectResponse>> GetProject(int projectID);
    }
}