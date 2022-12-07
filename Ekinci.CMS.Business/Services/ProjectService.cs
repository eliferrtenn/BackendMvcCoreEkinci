using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        public ProjectService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListProjectResponses>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectResponses>>();
            var projects = await (from proj in _context.Projects
                                  join ps in _context.ProjectStatus on proj.StatusID equals ps.ID
                                  select new ListProjectResponses
                                  {
                                      ID = proj.ID,
                                      StatusID = proj.StatusID,
                                      StatusName = ps.Name,
                                      Title = proj.Title,
                                      ThumbUrl = proj.ThumbUrl,
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
                                                          PhotoUrl = prph.PhotoUrl
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