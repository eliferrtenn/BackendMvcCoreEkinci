using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.CMS.Business.Models.Responses.ProjectStatusResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IProjectStatusService
    {
        Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll();
        Task<ServiceResult<GetProjectStatusResponse>> GetProjectStatus(int projectStatusID);

    }
}