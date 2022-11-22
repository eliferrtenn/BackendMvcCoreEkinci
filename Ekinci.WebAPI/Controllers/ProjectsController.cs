using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsService projectsService;

        public ProjectsController(IProjectsService ProjectsService)
        {
            projectsService = ProjectsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await projectsService.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetProject(int projectID)
        {
            var result = await projectsService.GetProject(projectID);
            if (result.IsSuccess)
            {
                TempData["MessageText"] = result.Message;
                return View(result.Data);
            }
            TempData["MessageText"] = result.Message;
            return View();
        }

    }
}