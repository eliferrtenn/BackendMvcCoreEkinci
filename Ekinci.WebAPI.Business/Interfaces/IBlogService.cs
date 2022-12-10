using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.BlogResponses;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IBlogService
    {
        Task<ServiceResult<List<ListBlogResponse>>> GetAll();
        Task<ServiceResult<GetBlogResponse>> GetBlog(int blogID);
    }
}