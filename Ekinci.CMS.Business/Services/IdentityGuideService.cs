using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IdentityGuideRequests;
using Ekinci.CMS.Business.Models.Responses.IdentityGuideResponses;
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
    public class IdentityGuideService : BaseService, IIdentityGuideService
    {
        const string file = "IdentityGuide/";

        public IdentityGuideService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult> AddIdentityGuide(AddIdentityGuideRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["IdentityGuideWithNameAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var identityGuide = new IdentityGuide();
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
                    identityGuide.PhotoUrl = type;
                }
            }
            identityGuide.Title = request.Title;
            _context.IdentityGuides.Add(identityGuide);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["IdentityGuideAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteIdentityGuide(DeleteIdentityGuideRequest request)
        {
            var result = new ServiceResult();
            var identityGuide = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (identityGuide == null)
            {
                result.SetError(_localizer["IdentityGuideNotFound"]);
                return result;
            }
            identityGuide.IsEnabled = false;
            _context.IdentityGuides.Update(identityGuide);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["IdentityGuideDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListIdentityGuideResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListIdentityGuideResponse>>();
            if (_context.IdentityGuides.Count() == 0)
            {
                result.SetError(_localizer["IdentityGuideNotFound"]);
                return result;
            }
            var identities = await (from identity in _context.IdentityGuides
                                    where identity.IsEnabled == true
                                    select new ListIdentityGuideResponse
                                    {
                                        ID = identity.ID,
                                        PhotoUrl = ekinciUrl + identity.PhotoUrl,
                                    }).ToListAsync();
            result.Data = identities;
            return result;
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide(int IdentityGuideID)
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var histories = await (from hist in _context.Histories
                                   where hist.ID == IdentityGuideID
                                   select new GetIdentityGuideResponse
                                   {
                                       ID = hist.ID,
                                       Title = hist.Title,
                                       PhotoUrl = ekinciUrl + hist.PhotoUrl,
                                   }).FirstAsync();
            if (histories == null)
            {
                result.SetError(_localizer["IdentityGuideNotFound"]);
                return result;
            }
            result.Data = histories;
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
                var history = await _context.IdentityGuides.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (history == null)
                {
                    result.SetError(_localizer["IdentityGuideNotFound"]);
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + history.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        history.PhotoUrl = type;
                    }
                }
                else
                {
                    history.PhotoUrl = request.PhotoUrl;
                }
                history.Title = request.Title;
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["IdentityGuideUpdated"]);
            }
            else
            {
                result.SetError(_localizer["IdentityGuideWithNameAlreadyExist"]);
            }
            return result;
        }
    }
}