using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ProjectRequests;
using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        const string fileThumb = "Project/Thumb/";
        const string file = "Project/General/";
        private readonly IProjectStatusService projectStatusService;

        public ProjectService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult> AddProject(AddProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Projects.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["ProjectWithNameAlreadyExist"]);
                return result;
            }
            var project = new Projects();
            if (PhotoUrl != null)
            {
                Guid guid = Guid.NewGuid();
                var filePaths = new List<string>();
                if (PhotoUrl.Length > 0)
                {
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = fileThumb + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    project.ThumbUrl = type;
                }
            }
            project.Title = request.Title;
            project.SubTitle = request.SubTitle;
            project.Description = request.Description;
            project.ProjectDate = request.ProjectDate;
            project.DeliveryDate = request.DeliveryDate;
            project.ApartmentCount = request.ApartmentCount;
            project.SquareMeter = request.SquareMeter;
            project.Longitude = request.Longitude;
            project.Latitude = request.Latitude;
            project.StatusID = request.StatusID;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            var id = project.ID;
            if (PhotoUrls != null)
            {
                foreach (var photo in PhotoUrls)
                {
                    var projectPhoto = new ProjectPhoto();
                    projectPhoto.ProjectID = id;
                    Guid guid = Guid.NewGuid();
                    var filePaths = new List<string>();
                    if (photo.Length > 0)
                    {
                        var path = Path.GetExtension(photo.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        projectPhoto.PhotoUrl = type;
                        _context.ProjectPhotos.Add(projectPhoto);
                    }
                }
            }
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["ProjectAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteProjectPhoto(int projectPhotoID)
        {
            var result = new ServiceResult();
            var projectPhoto = await _context.ProjectPhotos.FirstOrDefaultAsync(x => x.ID == projectPhotoID);
            if (projectPhoto == null)
            {
                result.SetError(_localizer["ProjectPhotoNotFound"]);
                return result;
            }
            projectPhoto.IsEnabled = false;
            _context.ProjectPhotos.Update(projectPhoto);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["ProjectPhotoDeleted"]);
            return result;
        }

        public async Task<ServiceResult> DeletProject(int projecttID)
        {
            var result = new ServiceResult();
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.ID == projecttID);
            if (project == null)
            {
                result.SetError(_localizer["ProjectNotFound"]);
                return result;
            }
            project.IsEnabled = false;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["ProjectDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListProjectResponses>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectResponses>>();
            var projects = await (from proj in _context.Projects
                                  where proj.IsEnabled==true
                                  join ps in _context.ProjectStatus on proj.StatusID equals ps.ID
                                  select new ListProjectResponses
                                  {
                                      ID = proj.ID,
                                      StatusID = proj.StatusID,
                                      StatusName = ps.Name,
                                      Title = proj.Title,
                                      ThumbUrl =ekinciUrl+proj.ThumbUrl,
                                      SubTitle = proj.SubTitle,
                                      Description = proj.Description,
                                      ProjectDate = proj.ProjectDate.ToFormattedDate(),
                                      DeliveryDate = proj.DeliveryDate.ToFormattedDate(),
                                      ApartmentCount = proj.ApartmentCount,
                                      SquareMeter = proj.SquareMeter,
                                  }).ToListAsync();
            //TODO:NULL OLMA İHTİMALİNİ VER
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
                                                      where prph.IsEnabled == true
                                                      select new ProjectPhotosResponse
                                                      {
                                                          ID = prph.ID,
                                                          PhotoUrl =ekinciUrl+prph.PhotoUrl
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
                                     ProjectDate = proj.ProjectDate,
                                     DeliveryDate = proj.DeliveryDate,
                                     ThumbUrl =ekinciUrl+proj.ThumbUrl,
                                     FileUrl = ekinciUrl + proj.FileUrl,
                                     ApartmentCount = proj.ApartmentCount,
                                     SquareMeter = proj.SquareMeter,
                                     ProjectPhotos = projectPhotos
                                 }).FirstAsync();
            if (project == null)
            {
                result.SetError(_localizer["ProjectNotFound"]);
                return result;
            }
            result.Data = project;
            return result;
        }

        public async Task<ServiceResult> UpdateProject(UpdateProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Projects.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == true)
            {
                result.SetError(_localizer["ProjectWithNameAlreadyExist"]);
                return result;
            }
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (project == null)
            {
                result.SetError(_localizer["ProjectNotFound"]);
                return result;
            }
            else
            {
                if (PhotoUrl != null)
                {
                    Guid guid = Guid.NewGuid();
                    var filePaths = new List<string>();
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + project.ThumbUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = fileThumb + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        project.ThumbUrl = type;
                    }
                }
                project.Title = request.Title;
                project.SubTitle = request.SubTitle;
                project.Description = request.Description;
                project.ProjectDate = request.ProjectDate;
                project.DeliveryDate = request.DeliveryDate;
                project.ApartmentCount = request.ApartmentCount;
                project.SquareMeter = request.SquareMeter;
                project.Longitude = request.Longitude;
                project.Latitude = request.Latitude;
                project.StatusID = request.StatusID;
                await _context.SaveChangesAsync();
                var id = project.ID;
                if (PhotoUrls != null)
                {
                    foreach (var photo in PhotoUrls)
                    {
                        var projectPhoto = new ProjectPhoto();
                        projectPhoto.ProjectID = id;
                        Guid guid = Guid.NewGuid();
                        var filePaths = new List<string>();
                        if (photo.Length > 0)
                        {
                            var path = Path.GetExtension(photo.FileName);
                            var type = file + guid.ToString() + path;
                            var filePath = "wwwroot/Dosya/" + type;
                            var filePathBunnyCdn = "/ekinci/" + type;
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                            projectPhoto.PhotoUrl = type;
                            _context.ProjectPhotos.Add(projectPhoto);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["ProjectUpdated"]);
                return result;
            }

        }
    }
}