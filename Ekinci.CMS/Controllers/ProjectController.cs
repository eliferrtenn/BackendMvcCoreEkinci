using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ProjectRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class ProjectController : CMSBaseController
    {
        private readonly IProjectService projectService;
        private readonly IProjectStatusService projectStatusService;

        public ProjectController(IProjectService _projectService, IProjectStatusService _projectStatusService)
        {
            projectService = _projectService;
            projectStatusService = _projectStatusService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await projectService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await projectService.GetProject(id);
            return View(result.Data);
        }
        public async Task<IActionResult> Create()
        {
            var result1 = await projectStatusService.GetAll();
            ViewBag.StatusID = new SelectList(result1.Data, "ID", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = await projectService.AddProject(request, PhotoUrls, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result1 = await projectStatusService.GetAll();
            ViewBag.StatusID = new SelectList(result1.Data, "ID", "Name");
            var result = await projectService.UpdateProject(id);
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = await projectService.UpdateProject(request, PhotoUrls, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAnnouncement(int id)
        {
            var result = await projectService.DeletProject(id);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
        [HttpPost]
        public async Task<JsonResult> DeletePhoto(int id)
        {
            var result = await projectService.DeleteProjectPhoto(id);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }

    }
}
