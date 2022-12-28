using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.BlogResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        const string file = "Blog/";
        public BlogService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListBlogResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListBlogResponse>>();
            var blogs = await (from blog in _context.Blog
                               select new ListBlogResponse
                               {
                                   ID = blog.ID,
                                   Title = blog.Title,
                                   BlogDate = blog.BlogDate.ToFormattedDate(),
                                   InstagramUrl = blog.InstagramUrl,
                                   PhotoUrl =blog.PhotoUrl.PrepareCDNUrl(file),
                               }).ToListAsync();
            result.Data = blogs;
            return result;
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog(int blogID)
        {
            var result = new ServiceResult<GetBlogResponse>();
            var blogs = await (from blog in _context.Blog
                               where blog.ID == blogID
                               select new GetBlogResponse
                               {
                                   ID = blog.ID,
                                   Title = blog.Title,
                                   BlogDate = blog.BlogDate.ToFormattedDate(),
                                   InstagramUrl = blog.InstagramUrl,
                                   PhotoUrl =blog.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            if (blogs == null)
            {
                result.SetError("Blog bulunamadı");
                return result;
            }
            result.Data = blogs;
            return result;
        }
    }
}