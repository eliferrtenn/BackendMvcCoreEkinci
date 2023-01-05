using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Responses.BlogResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        const string file = "Blog/";

        public BlogService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddBlog(AddBlogRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Blog.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAdded"]);
                return result;
            }
            var Blog = new Blog();
            var fileUploadResult = _fileUpload.Upload(PhotoUrl, file);
            if (!fileUploadResult.IsSuccess)
            {
                result.SetError(_localizer["PhotoCouldNotUploaded"]);
                return result;
            }
            Blog.PhotoUrl = fileUploadResult.FileName;
            Blog.Title = request.Title;
            Blog.BlogDate = request.BlogDate;
            Blog.InstagramUrl = request.InstagramUrl;
            _context.Blog.Add(Blog);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> UpdateBlog(UpdateBlogRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Blog.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var blog = await _context.Blog.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (blog == null)
                {
                    result.SetError(_localizer["RecordNotFound"]);
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        var fileUploadResult = _fileUpload.Upload(PhotoUrl, file);
                        if (!fileUploadResult.IsSuccess)
                        {
                            result.SetError(_localizer["PhotoCouldNotUploaded"]);
                            return result;
                        }
                        blog.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                blog.Title = request.Title;
                blog.BlogDate = request.BlogDate;
                blog.InstagramUrl = request.InstagramUrl;
                _context.Blog.Update(blog);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["RecordUpdated"]);
            }
            else
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
            }
            return result;
        }

        public async Task<ServiceResult> DeleteBlog(DeleteBlogRequest request)
        {
            var result = new ServiceResult();
            var blog = await _context.Blog.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (blog == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            blog.IsEnabled = false;
            _context.Blog.Update(blog);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListBlogResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListBlogResponse>>();
            if (_context.Blog.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var blogs = await (from blog in _context.Blog
                               where blog.IsEnabled == true
                               select new ListBlogResponse
                               {
                                   ID = blog.ID,
                                   Title = blog.Title,
                                   BlogDate = blog.BlogDate.ToFormattedDate(),
                                   InstagramUrl = blog.InstagramUrl,
                                   PhotoUrl = blog.PhotoUrl.PrepareCDNUrl(file),
                               }).ToListAsync();
            result.Data = blogs;
            return result;
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog(int BlogID)
        {
            var result = new ServiceResult<GetBlogResponse>();
            var blogs = await (from blog in _context.Blog
                               where blog.ID == BlogID
                               select new GetBlogResponse
                               {
                                   ID = blog.ID,
                                   Title = blog.Title,
                                   BlogDate = blog.BlogDate.ToFormattedDate(),
                                   InstagramUrl = blog.InstagramUrl,
                                   PhotoUrl = blog.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            if (blogs == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = blogs;
            return result;
        }

        public async Task<ServiceResult<UpdateBlogRequest>> UpdateBlog(int BlogID)
        {
            var result = new ServiceResult<UpdateBlogRequest>();
            var blogs = await (from blog in _context.Blog
                               where blog.ID == BlogID
                               select new UpdateBlogRequest
                               {
                                   ID = blog.ID,
                                   Title = blog.Title,
                                   BlogDate = blog.BlogDate,
                                   InstagramUrl = blog.InstagramUrl,
                                   PhotoUrl = blog.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            if (blogs == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = blogs;
            return result;
        }
    }
}