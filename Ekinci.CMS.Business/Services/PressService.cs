using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Ekinci.CMS.Business.Models.Responses.PressResponses;
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
    public class PressService : BaseService, IPressService
    {
        const string file = "Press/";

        public PressService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddPress(AddPressRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            //TODO:Buraya bir daha bak
            var exist = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            var filePaths = new List<string>();
            Guid guid = Guid.NewGuid();
            var press = new Press();
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
                    press.PhotoUrl = fileUploadResult.FileName;
                }
            }
            _context.Press.Add(press);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeletePress(DeletePressRequest request)
        {
            var result = new ServiceResult();
            var press = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (press == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }

            press.IsEnabled = false;
            _context.Press.Update(press);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListPressesResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListPressesResponse>>();
            if (_context.Press.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var presses = await (from press in _context.Press
                                 where press.IsEnabled == true
                                 select new ListPressesResponse
                                 {
                                     ID = press.ID,
                                     PhotoUrl = press.PhotoUrl.PrepareCDNUrl(file),
                                 }).ToListAsync();
            result.Data = presses;
            return result;
        }

        public async Task<ServiceResult<GetPressResponse>> GetPress(int PressID)
        {
            var result = new ServiceResult<GetPressResponse>();
            var press = await (from pres in _context.Press
                               where pres.ID == PressID
                               select new GetPressResponse
                               {
                                   ID = pres.ID,
                                   PhotoUrl = pres.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            if (press == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = press;
            return result;
        }

        public async Task<ServiceResult> UpdatePress(UpdatePressRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var press = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (press == null)
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
                    press.PhotoUrl = fileUploadResult.FileName;
                }
            }
            else
            {
                press.PhotoUrl = request.PhotoUrl;
            }
            _context.Press.Update(press);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordUpdated"]);
            return result;
        }
    }
}