using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class AnnouncementController : APIBaseController
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
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAnnouncement(int announcementID)
        {
            var result = await announcementService.GetAnnouncement(announcementID);
            return Ok(result);
        }
    }
}