using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.ProjectStatusResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class ProjectStatusService : BaseService, IProjectStatusService
    {
        const string file = "ProjectStatus/";

        public ProjectStatusService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectStatusResponse>>();
            var blogs = await (from blog in _context.ProjectStatus
                               select new ListProjectStatusResponse
                               {
                                   ID = blog.ID,
                                   Name = blog.Name,
                                   PhotoUrl = blog.PhotoUrl.PrepareCDNUrl(file),
                               }).ToListAsync();
            result.Data = blogs;
            return result;
        }
    }
}