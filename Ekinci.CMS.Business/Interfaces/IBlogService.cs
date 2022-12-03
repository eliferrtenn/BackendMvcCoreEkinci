using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Responses.BlogResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IBlogService
    {
        Task<ServiceResult> UpdateBlog(UpdateBlogRequest request);
        Task<ServiceResult<GetBlogResponse>> GetBlog();
    }
}