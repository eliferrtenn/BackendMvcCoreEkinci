using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
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
    public class IntroService : BaseService, IIntroService
    {
        const string file = "Intro/";

        public IntroService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult> AddIntro(AddIntroRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Intros.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["IntroWithNameAlreadyExist"]);
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var intro = new Intro();
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
                    intro.PhotoUrl = type;
                }
            }
            intro.Title = request.Title;
            intro.Description = request.Description;
            intro.SquareMeter = request.SquareMeter;
            intro.YearCount = request.YearCount;
            intro.CommercialAreaCount = request.CommercialAreaCount;
            _context.Intros.Add(intro);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["IntroAdded"]);
            return result;
        }

        public async Task<ServiceResult<GetIntroResponse>> GetIntro()
        {
            var result = new ServiceResult<GetIntroResponse>();
            if (_context.Intros.Count() == 0)
            {
                result.SetError(_localizer["IntoNotFound"]);
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
                                   PhotoUrl = ekinciUrl + intro.PhotoUrl,
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
                    result.SetError(_localizer["IntroNotFound"]);
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + intro.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        intro.PhotoUrl = type;
                    }
                }
                intro.Title = request.Title;
                intro.Description = request.Description;
                intro.SquareMeter = request.SquareMeter;
                intro.YearCount = request.YearCount;
                intro.CommercialAreaCount = request.CommercialAreaCount;
                _context.Intros.Update(intro);
                await _context.SaveChangesAsync();
                result.SetSuccess(_localizer["IntroUpdated"]);
            }
            else
            {
                result.SetError(_localizer["IntroWithNameAlreadyExist"]);
            }
            return result;
        }
    }
}