using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.ProjectResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class ProjectsService : BaseService, IProjectsService
    {
        public ProjectsService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListProjectsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectsResponse>>();
            var projects = await (from proj in _context.Projects
                                  select new ListProjectsResponse
                                  {
                                      ID = proj.ID,
                                      Status = proj.Status,
                                      Title = proj.Title,
                                      SubTitle = proj.SubTitle,
                                      Description = proj.Description,
                                      ProjectDate = proj.ProjectDate,
                                      DeliveryDate = proj.DeliveryDate,
                                      ApartmentCount = proj.ApartmentCount,
                                      SquareMeter = proj.SquareMeter,
                                  }).ToListAsync();
            result.Data = projects;
            return result;
        }

        public async Task<ServiceResult<GetProjectResponse>> GetProject(int projectID)
        {
            var result = new ServiceResult<GetProjectResponse>();
            var project = await (from proj in _context.Projects
                                 where proj.ID == projectID
                                 select new GetProjectResponse
                                 {
                                     ID = proj.ID,
                                     Status = proj.Status,
                                     Title = proj.Title,
                                     SubTitle = proj.SubTitle,
                                     Description = proj.Description,
                                     ProjectDate = proj.ProjectDate,
                                     DeliveryDate = proj.DeliveryDate,
                                     ApartmentCount = proj.ApartmentCount,
                                     SquareMeter = proj.SquareMeter,
                                 }).FirstAsync();

            result.Data = project;
            return result;
        }
    }
}