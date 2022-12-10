using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.VideosRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideosService videosService;

        public VideoController(IVideosService _videosService)
        {
            videosService = _videosService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await videosService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await videosService.GetVideo(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddVideosRequest request)
        {
            var result = await videosService.AddVideo(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await videosService.GetVideo(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateVideosRequest request)
        {
            var result = await videosService.UpdateVideo(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteVideosRequest request)
        {
            var result = await videosService.DeleteVideo(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}