using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.CMS.Business.Models.Responses.SustainabilityResponses;
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

namespace Ekinci.CMS.Business.Services
{
    public class SustainabilityService : BaseService, ISustainabilityService
    {
        const string file = "Sustainability/";

        public SustainabilityService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddSustainability(AddSustainabilityRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Sustainabilities.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var sustainability = new Sustainability();
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
                    sustainability.PhotoUrl = fileUploadResult.FileName;
                }
            }
            sustainability.Title = request.Title;
            sustainability.Description = request.Description;
            _context.Sustainabilities.Add(sustainability);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }
        public async Task<ServiceResult<GetSustainabilityResponse>> GetSustainability()
        {
            var result = new ServiceResult<GetSustainabilityResponse>();

            if (_context.Sustainabilities.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }

            var sustainability = await (from sustain in _context.Sustainabilities
                                        where sustain.IsEnabled == true
                                        select new GetSustainabilityResponse
                                        {
                                            ID = sustain.ID,
                                            Title = sustain.Title,
                                            Description = sustain.Description,
                                            PhotoUrl = sustain.PhotoUrl.PrepareCDNUrl(file),
                                        }).FirstAsync();
            result.Data = sustainability;
            return result;
        }

        public async Task<ServiceResult> UpdateSustainability(UpdateSustainabilityRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var sustainability = await _context.Sustainabilities.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (sustainability == null)
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
                        sustainability.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                sustainability.Title = request.Title;
                sustainability.Description = request.Description;
                _context.Sustainabilities.Update(sustainability);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["RecordUpdated"]);
            }
            return result;
        }

    }
}