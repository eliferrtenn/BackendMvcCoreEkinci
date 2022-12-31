using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class ProjectStatusController : APIBaseController
    {
        private readonly IProjectStatusService projectStatusService;

        public ProjectStatusController(IProjectStatusService ProjectStatusService)
        {
            projectStatusService = ProjectStatusService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await projectStatusService.GetAll();
            return Ok(result);
        }
    }
}