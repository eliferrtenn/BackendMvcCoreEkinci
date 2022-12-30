using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.CMS.Business.Models.Responses.IdentityGuideResponses;
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
    public class IdentityGuideService : BaseService, IIdentityGuideService
    {
        const string file = "IdentityGuide/";

        public IdentityGuideService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddIdentityGuide(AddIdentityGuideRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var identityGuide = new IdentityGuide();
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
                    identityGuide.PhotoUrl = fileUploadResult.FileName;
                }
            }
            identityGuide.Title = request.Title;
            _context.IdentityGuides.Add(identityGuide);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteIdentityGuide(DeleteIdentityGuideRequest request)
        {
            var result = new ServiceResult();
            var identityGuide = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (identityGuide == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            identityGuide.IsEnabled = false;
            _context.IdentityGuides.Update(identityGuide);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListIdentityGuideResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListIdentityGuideResponse>>();
            if (_context.IdentityGuides.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var identityGuides = await (from identity in _context.IdentityGuides
                                    where identity.IsEnabled == true
                                    select new ListIdentityGuideResponse
                                    {
                                        ID = identity.ID,
                                        Title=identity.Title,
                                        PhotoUrl = identity.PhotoUrl.PrepareCDNUrl(file),
                                    }).ToListAsync();
            result.Data = identityGuides;
            return result;
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int IdentityGuideID)
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var identityGuide = await (from identit in _context.IdentityGuides
                                   where identit.ID == IdentityGuideID
                                   select new GetIdentityGuideResponse
                                   {
                                       ID = identit.ID,
                                       Title = identit.Title,
                                       PhotoUrl = identit.PhotoUrl.PrepareCDNUrl(file),
                                   }).FirstAsync();
            if (identityGuide == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = identityGuide;
            return result;
        }

        public async Task<ServiceResult> UpdateIdentityGuide(UpdateIdentityGuideRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.IdentityGuides.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var identityGuide = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (identityGuide == null)
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
                        identityGuide.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                identityGuide.Title = request.Title;
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