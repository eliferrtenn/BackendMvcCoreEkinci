using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    [Authorize]
    public class BlogController : CMSBaseController
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService _blogService)
        {
            blogService = _blogService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await blogService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await blogService.GetBlog(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddBlogRequest request, IFormFile PhotoUrl)
        {
            var result = await blogService.AddBlog(request, PhotoUrl);
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
            var result = await blogService.UpdateBlog(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateBlogRequest request, IFormFile PhotoUrl)
        {
            var result = await blogService.UpdateBlog(request, PhotoUrl);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteBlogRequest request)
        {
            var result = await blogService.DeleteBlog(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }

    }
}