using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.CMS.Business.Models.Responses.ProjectStatusResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class ProjectStatusService : BaseService, IProjectStatusService
    {
        public ProjectStatusService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectStatusResponse>>();
            if (_context.ProjectStatus.Count() == 0)
            {
                result.SetError("Proje Durumu bulunamadı");
                return result;
            }
            var statuses = await (from status in _context.ProjectStatus
                                  select new ListProjectStatusResponse
                                  {
                                      ID = status.ID,
                                      Name = status.Name,
                                  }).ToListAsync();
            result.Data = statuses;
            return result;
        }

        public async Task<ServiceResult<GetProjectStatusResponse>> GetProjectStatus(int projectStatusID)
        {
            var result = new ServiceResult<GetProjectStatusResponse>();
            var statuses = await(from status in _context.ProjectStatus
                                  where status.ID == projectStatusID
                                  select new GetProjectStatusResponse
                                  {
                                      ID = status.ID,
                                      Name = status.Name,
                                  }).FirstAsync();
            if (statuses == null)
            {
                result.SetError("Proje Durumu bulunamadı");
                return result;
            }
            result.Data = statuses;
            return result;
        }
    }
}