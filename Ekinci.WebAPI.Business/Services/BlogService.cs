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
        public BlogService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListBlogResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListBlogResponse>>();
            var blogs = await (from identity in _context.Blog
                               select new ListBlogResponse
                               {
                                   ID = identity.ID,
                                   Title = identity.Title,
                                   BlogDate = identity.BlogDate.ToFormattedDate(),
                                   InstagramUrl = identity.InstagramUrl,
                                   PhotoUrl =ekinciUrl+identity.PhotoUrl,
                               }).ToListAsync();
            result.Data = blogs;
            return result;
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog(int blogID)
        {
            var result = new ServiceResult<GetBlogResponse>();
            var blogs = await (from identity in _context.Blog
                               where identity.ID == blogID
                               select new GetBlogResponse
                               {
                                   ID = identity.ID,
                                   Title = identity.Title,
                                   BlogDate = identity.BlogDate.ToFormattedDate(),
                                   InstagramUrl = identity.InstagramUrl,
                                   PhotoUrl =ekinciUrl+identity.PhotoUrl,
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