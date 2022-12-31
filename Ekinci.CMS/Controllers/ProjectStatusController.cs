using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ProjectStatusRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class ProjectStatusController : CMSBaseController
    {
        private readonly IProjectStatusService projectStatusService;

        public ProjectStatusController(IProjectStatusService _projectStatusService)
        {
            projectStatusService = _projectStatusService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await projectStatusService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await projectStatusService.GetProjectStatus(id);
            return View(result.Data);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var result = await projectStatusService.UpdateProjectStatus(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProjectStatusRequest request, IFormFile PhotoUrl)
        {
            var result = await projectStatusService.UpdateProjectStatus(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
    }
}