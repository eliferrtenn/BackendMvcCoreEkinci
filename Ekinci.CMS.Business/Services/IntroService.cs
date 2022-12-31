using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
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
    public class IntroService : BaseService, IIntroService
    {
        const string file = "Intro/";

        public IntroService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddIntro(AddIntroRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Intros.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var intro = new Intro();
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
                    intro.PhotoUrl = fileUploadResult.FileName;
                }
            }
            intro.Title = request.Title;
            intro.Description = request.Description;
            intro.SquareMeter = request.SquareMeter;
            intro.YearCount = request.YearCount;
            intro.CommercialAreaCount = request.CommercialAreaCount;
            _context.Intros.Add(intro);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult<GetIntroResponse>> GetIntro()
        {
            var result = new ServiceResult<GetIntroResponse>();
            if (_context.Intros.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var Intro = await (from intro in _context.Intros
                               select new GetIntroResponse
                               {
                                   ID = intro.ID,
                                   Title = intro.Title,
                                   Description = intro.Description,
                                   SquareMeter = intro.SquareMeter,
                                   YearCount = intro.YearCount,
                                   CommercialAreaCount = intro.CommercialAreaCount,
                                   PhotoUrl = intro.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            result.Data = Intro;
            return result;
        }

        public async Task<ServiceResult> UpdateIntro(UpdateIntroRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var intro = await _context.Intros.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (intro == null)
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
                        intro.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                intro.Title = request.Title;
                intro.Description = request.Description;
                intro.SquareMeter = request.SquareMeter;
                intro.YearCount = request.YearCount;
                intro.CommercialAreaCount = request.CommercialAreaCount;
                _context.Intros.Update(intro);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["RecordUpdated"]);
            }
            else
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
            }
            return result;
        }

        public async Task<ServiceResult<UpdateIntroRequest>> UpdateIntro()
        {
            var result = new ServiceResult<UpdateIntroRequest>();
            if (_context.Intros.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var Intro = await (from intro in _context.Intros
                               select new UpdateIntroRequest
                               {
                                   ID = intro.ID,
                                   Title = intro.Title,
                                   Description = intro.Description,
                                   SquareMeter = intro.SquareMeter,
                                   YearCount = intro.YearCount,
                                   CommercialAreaCount = intro.CommercialAreaCount,
                                   PhotoUrl = intro.PhotoUrl.PrepareCDNUrl(file),
                               }).FirstAsync();
            result.Data = Intro;
            return result;
        }
    }
}