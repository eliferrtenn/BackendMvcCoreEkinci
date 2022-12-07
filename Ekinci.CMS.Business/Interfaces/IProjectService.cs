using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult<List<ListProjectResponses>>> GetAll();
        Task<ServiceResult<GetProjectResponse>> GetProject(int projectID);
    }
}