using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService announcementService;

        public AnnouncementController(IAnnouncementService AnnouncementService)
        {
            announcementService = AnnouncementService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await announcementService.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAnnouncement(int announcementID)
        {
            var result = await announcementService.GetAnnouncement(announcementID);
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