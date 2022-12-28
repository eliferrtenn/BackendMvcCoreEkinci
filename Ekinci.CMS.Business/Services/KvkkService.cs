using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.KvkkRequests;
using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.CMS.Business.Models.Responses.KvkkResponses;
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
    public class KvkkService : BaseService, IKvkkService
    {
        const string file = "Kvkk/";

        public KvkkService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddKvkk(AddKvkkRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Kvkks.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var kvkk = new Kvkk();
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
                    kvkk.PhotoUrl = fileUploadResult.FileName;
                }
            }
            kvkk.Title = request.Title;
            kvkk.Description = request.Description;
            _context.Kvkks.Add(kvkk);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult<GetKvkkResponse>> GetKvkk()
        {
            var result = new ServiceResult<GetKvkkResponse>();
            if (_context.Kvkks.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var kvkk = await (from kvk in _context.Kvkks
                              select new GetKvkkResponse
                              {
                                  ID = kvk.ID,
                                  Title = kvk.Title,
                                  Description = kvk.Description,
                                  PhotoUrl = kvk.PhotoUrl.PrepareCDNUrl(file),
                              }).SingleOrDefaultAsync();

            result.Data = kvkk;
            return result;
        }

        public async Task<ServiceResult> UpdateKvkk(UpdateKvkkResponse request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var kvkk = await _context.Kvkks.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (kvkk == null)
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
                    kvkk.PhotoUrl = fileUploadResult.FileName;
                }
            }
            kvkk.Title = request.Title;
            kvkk.Description = request.Description;
            _context.Kvkks.Update(kvkk);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordUpdated"]);
            return result;
        }
    }
}