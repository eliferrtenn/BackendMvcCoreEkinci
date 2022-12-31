using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Responses.BlogResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IBlogService
    {
        Task<ServiceResult> AddBlog(AddBlogRequest request, IFormFile PhotoUrl);
        Task<ServiceResult<UpdateBlogRequest>> UpdateBlog(int BlogID);
        Task<ServiceResult> UpdateBlog(UpdateBlogRequest request, IFormFile PhotoUrl);
        Task<ServiceResult> DeleteBlog(DeleteBlogRequest request);
        Task<ServiceResult<List<ListBlogResponse>>> GetAll();
        Task<ServiceResult<GetBlogResponse>> GetBlog(int BlogID);
    }
}