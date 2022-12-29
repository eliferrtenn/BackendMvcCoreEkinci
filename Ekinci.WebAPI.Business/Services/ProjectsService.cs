using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.ProjectResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class ProjectsService : BaseService, IProjectsService
    {
        const string fileThumb = "Project/Thumb/";
        const string file = "Project/General/";

        public ProjectsService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<List<ListProjectsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectsResponse>>();
            var projects = await (from proj in _context.Projects
                                  join ps in _context.ProjectStatus on proj.StatusID equals ps.ID
                                  select new ListProjectsResponse
                                  {
                                      ID = proj.ID,
                                      StatusID = proj.StatusID,
                                      StatusName = ps.Name,
                                      Title = proj.Title,
                                      ThumbUrl = proj.ThumbUrl.PrepareCDNUrl(fileThumb),
                                      SubTitle = proj.SubTitle,
                                      Description = proj.Description,
                                      ProjectDate = proj.ProjectDate.ToFormattedDate(),
                                      DeliveryDate = proj.DeliveryDate.ToFormattedDate(),
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
                                 join ps in _context.ProjectStatus on proj.StatusID equals ps.ID
                                 let projectPhotos = (from prph in _context.ProjectPhotos
                                                      where prph.ProjectID == proj.ID
                                                      select new ProjectPhotosResponse
                                                      {
                                                          ID = prph.ID,
                                                          PhotoUrl = prph.PhotoUrl.PrepareCDNUrl(file)
                                                      }).ToList()
                                 where proj.ID == projectID
                                 select new GetProjectResponse
                                 {
                                     ID = proj.ID,
                                     StatusID = proj.StatusID,
                                     StatusName = ps.Name,
                                     Title = proj.Title,
                                     SubTitle = proj.SubTitle,
                                     Description = proj.Description,
                                     ProjectDate = proj.ProjectDate.ToFormattedDate(),
                                     DeliveryDate = proj.DeliveryDate.ToFormattedDate(),
                                     ApartmentCount = proj.ApartmentCount,
                                     SquareMeter = proj.SquareMeter,
                                     ProjectPhotos = projectPhotos
                                 }).FirstAsync();
            if (project == null)
            {
                result.SetError("Proje bulunamadı");
                return result;
            }
            result.Data = project;
            return result;
        }
    }
}