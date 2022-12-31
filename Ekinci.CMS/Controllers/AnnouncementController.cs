using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AnnouncementRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class AnnouncementController : CMSBaseController
    {
        private readonly IAnnouncementService announcementService;


        public AnnouncementController(IAnnouncementService _announcementService)
        {
            announcementService = _announcementService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await announcementService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await announcementService.GetAnnouncement(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            var model = new AddAnnouncementRequest();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddAnnouncementRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = await announcementService.AddAnnouncement(request, PhotoUrls, PhotoUrl);
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
            var result = await announcementService.UpdateAnnouncement(id);
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAnnouncementRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = await announcementService.UpdateAnnouncement(request, PhotoUrls, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            var errorResultData = await announcementService.GetAnnouncement(request.ID);
            Message(result);
            return View(errorResultData.Data);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAnnouncement(int id)
        {
            var result = await announcementService.DeleteAnnouncement(id);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
        [HttpPost]
        public async Task<JsonResult> DeletePhoto(int id)
        {
            var result = await announcementService.DeleteAnnouncementPhoto(id);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }

    }
}