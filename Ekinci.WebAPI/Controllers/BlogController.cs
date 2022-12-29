using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class BlogController : APIBaseController
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService BlogService)
        {
            blogService = BlogService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await blogService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommercialArea(int BlogID)
        {
            var result = await blogService.GetBlog(BlogID);
            return Ok(result);
        }

    }
}