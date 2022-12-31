using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ProjectStatusRequests;
using Ekinci.CMS.Business.Models.Responses.ProjectStatusResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class ProjectStatusService : BaseService, IProjectStatusService
    {
        const string file = "ProjectStatus/";

        public ProjectStatusService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult<List<ListProjectStatusResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListProjectStatusResponse>>();
            if (_context.ProjectStatus.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var statuses = await (from status in _context.ProjectStatus
                                  select new ListProjectStatusResponse
                                  {
                                      ID = status.ID,
                                      Name = status.Name,
                                      PhotoUrl = status.PhotoUrl.PrepareCDNUrl(file),
                                  }).ToListAsync();
            result.Data = statuses;
            return result;
        }

        public async Task<ServiceResult<GetProjectStatusResponse>> GetProjectStatus(int projectStatusID)
        {
            var result = new ServiceResult<GetProjectStatusResponse>();
            var statuses = await (from status in _context.ProjectStatus
                                  where status.ID == projectStatusID
                                  select new GetProjectStatusResponse
                                  {
                                      ID = status.ID,
                                      Name = status.Name,
                                      PhotoUrl = status.PhotoUrl.PrepareCDNUrl(file),
                                  }).FirstAsync();
            if (statuses == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = statuses;
            return result;
        }

        public async Task<ServiceResult> UpdateProjectStatus(UpdateProjectStatusRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var projectStatus = await _context.ProjectStatus.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (projectStatus == null)
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
                    projectStatus.PhotoUrl = fileUploadResult.FileName;
                }
            }
            projectStatus.Name= request.Name;
            _context.ProjectStatus.Update(projectStatus);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordUpdated"]);
            return result;
        }

        public async Task<ServiceResult<UpdateProjectStatusRequest>> UpdateProjectStatus(int projectStatusID)
        {
            var result = new ServiceResult<UpdateProjectStatusRequest>();
            var statuses = await (from status in _context.ProjectStatus
                                  where status.ID == projectStatusID
                                  select new UpdateProjectStatusRequest
                                  {
                                      ID = status.ID,
                                      Name = status.Name,
                                      PhotoUrl = status.PhotoUrl.PrepareCDNUrl(file),
                                  }).FirstAsync();
            if (statuses == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = statuses;
            return result;
        }
    }
}