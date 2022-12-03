using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Responses.BlogResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        public BlogService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> UpdateBlog(UpdateBlogRequest request)
        {
            var result = new ServiceResult();
            var blog = await _context.Blog.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (blog == null)
            {
                result.SetError("Blog Bulunamadı!");
                return result;
            }

            blog.Title = request.Title;
            blog.BlogDate = request.BlogDate;
            blog.InstagramUrl = request.InstagramUrl;
            //TODO:Blogdaki fotoğrafı güncelleme işlemi yapılacak
            _context.Blog.Update(blog);
            await _context.SaveChangesAsync();
            result.SetSuccess("Blog başarıyla güncellendi!");
            return result;
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog()
        {
            var result = new ServiceResult<GetBlogResponse>();
            var blog = await (from blo in _context.Blog
                              select new GetBlogResponse
                              {
                                  ID = blo.ID,
                                  Title = blo.Title,
                                  BlogDate = blo.BlogDate.ToFormattedDate(),
                                  PhotoUrl = blo.PhotoUrl,
                                  InstagramUrl = blo.InstagramUrl,
                                  //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                              }).FirstAsync();

            result.Data = blog;
            return result;
        }
    }
}