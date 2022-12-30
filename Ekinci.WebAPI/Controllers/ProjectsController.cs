using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class ProjectsController : APIBaseController
    {
        private readonly IProjectsService projectsService;

        public ProjectsController(IProjectsService ProjectsService)
        {
            projectsService = ProjectsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int projectID)
        {
            var result = await projectsService.GetAll(projectID);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProject(int projectID)
        {
            var result = await projectsService.GetProject(projectID);
            return Ok(result);
        }

    }
}