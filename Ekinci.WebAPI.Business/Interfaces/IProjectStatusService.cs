using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.ProjectResponse;
using Ekinci.WebAPI.Business.Models.Responses.ProjectStatusResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IProjectStatusService
    {
        Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll();
    }
}
