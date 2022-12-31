using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.CMS.Business.Models.Responses.CommercialAreaResponses;
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
    public class CommercialAreaService : BaseService, ICommercialAreaService
    {
        const string file = "CommercialArea/";

        public CommercialAreaService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddCommercialArea(AddCommercialAreaRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            var commercialArea = new CommercialArea();
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
                    commercialArea.PhotoUrl = fileUploadResult.FileName;
                }
            }
            commercialArea.Title = request.Title;
            _context.CommercialAreas.Add(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteCommercialArea(DeleteCommercialAreaRequest request)
        {
            var result = new ServiceResult();
            var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (commercialArea == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }

            commercialArea.IsEnabled = false;
            _context.CommercialAreas.Update(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListCommercialAreasResponse>>();
            if (_context.CommercialAreas.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.IsEnabled == true
                                     select new ListCommercialAreasResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl.PrepareCDNUrl(file),
                                     }).ToListAsync();
            result.Data = commercials;
            return result; ;
        }

        public async Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID)
        {
            var result = new ServiceResult<GetCommercialAreaResponse>();

            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.ID == CommercialAreaID
                                     select new GetCommercialAreaResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl.PrepareCDNUrl(file),
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = commercials;
            return result;
        }


        public async Task<ServiceResult> UpdateCommercialArea(UpdateCommercialAreaRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (commercialArea == null)
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
                        commercialArea.PhotoUrl = fileUploadResult.FileName;
                    }
                }
                commercialArea.Title = request.Title;
                _context.CommercialAreas.Update(commercialArea);
                await _context.SaveChangesAsync();

                result.SetSuccess(_localizer["RecordUpdated"]);
            }
            else
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
            }
            return result;
        }

        public async Task<ServiceResult<UpdateCommercialAreaRequest>> UpdateCommercialArea(int CommercialAreaID)
        {
            var result = new ServiceResult<UpdateCommercialAreaRequest>();

            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.ID == CommercialAreaID
                                     select new UpdateCommercialAreaRequest
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl.PrepareCDNUrl(file),
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = commercials;
            return result;
        }
    }
}