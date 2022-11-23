using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class VideosController : Controller
    {
        private readonly IVideosService videosService;

        public VideosController(IVideosService VideosService)
        {
            videosService = VideosService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await videosService.GetAll();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetVideo(int videoID)
        {
            var result = await videosService.GetVideo(videoID);
            return Ok(result);
        }
    }
}