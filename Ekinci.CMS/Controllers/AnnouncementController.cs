using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class AnnouncementController : Controller
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
            return View();
        } 
        }
}
