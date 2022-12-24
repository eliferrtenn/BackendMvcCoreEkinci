using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.CMS.Business.Models.Responses.CommercialAreaResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class CommercialAreaService : BaseService, ICommercialAreaService
    {
        const string file = "CommercialArea/";

        public CommercialAreaService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
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
                result.SetError(_localizer["CommercialAreaWithNameAlreadyExist"]);
                return result;
            }
            var commercialArea = new CommercialArea();
            if (PhotoUrl != null)
            {
                if (PhotoUrl.Length > 0)
                {
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = file + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    commercialArea.PhotoUrl = type;
                }
            }
            commercialArea.Title = request.Title;
            _context.CommercialAreas.Add(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["CommercialAreaAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteCommercialArea(DeleteCommercialAreaRequest request)
        {
            var result = new ServiceResult();
            var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (commercialArea == null)
            {
                result.SetError(_localizer["CommercialAreaNotFound"]);
                return result;
            }

            commercialArea.IsEnabled = false;
            _context.CommercialAreas.Update(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["CommercialAreaDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListCommercialAreasResponse>>();
            if (_context.CommercialAreas.Count() == 0)
            {
                result.SetError(_localizer["CommercialAreaNotFound"]);
                return result;
            }
            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.IsEnabled == true
                                     select new ListCommercialAreasResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = ekinciUrl + commercial.PhotoUrl,
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
                                         PhotoUrl = ekinciUrl + commercial.PhotoUrl,
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError(_localizer["CommercialAreaNotFound"]);
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
                    result.SetError(_localizer["CommercialAreaNotFound"]);
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + commercialArea.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        commercialArea.PhotoUrl = type;
                    }
                }
                commercialArea.Title = request.Title;
                _context.CommercialAreas.Update(commercialArea);
                await _context.SaveChangesAsync();

                result.SetSuccess(_localizer["CommercialAreaUpdated"]);
            }
            else
            {
                result.SetError(_localizer["CommercialAreaWithNameAlreadyExist"]);
            }
            return result;
        }
    }
}