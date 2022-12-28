using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.CMS.Business.Models.Responses.HumanResourceResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Reflection.Metadata;

namespace Ekinci.CMS.Business.Services
{
    public class HumanResourceService : BaseService, IHumanResourceService
    {
        const string file = "HumanResource/";

        public HumanResourceService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddHumanResource(AddHumanResourceRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.HumanResources.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var humanResource = new HumanResource();
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
                    humanResource.PhotoUrl = fileUploadResult.FileName;
                }
            }
            humanResource.Title = request.Title;
            humanResource.Description = request.Description;
            _context.HumanResources.Add(humanResource);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult<GetHumanResourceResponse>> GetHumanResource()
        {
            var result = new ServiceResult<GetHumanResourceResponse>();
            if (_context.HumanResources.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var humanResource = await (from human in _context.HumanResources
                                       select new GetHumanResourceResponse
                                       {
                                           ID = human.ID,
                                           Title = human.Title,
                                           Description = human.Description,
                                           PhotoUrl = human.PhotoUrl.PrepareCDNUrl(file),
                                       }).FirstAsync();
            result.Data = humanResource;
            return result;
        }

        public async Task<ServiceResult> UpdateHumanResource(UpdateHumanResourceRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var humanResource = await _context.HumanResources.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (humanResource == null)
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
                        humanResource.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                humanResource.Title = request.Title;
                humanResource.Description = request.Description;
                _context.HumanResources.Update(humanResource);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["RecordUpdated"]);
            }
            else
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
            }
            return result;
        }
    }
}