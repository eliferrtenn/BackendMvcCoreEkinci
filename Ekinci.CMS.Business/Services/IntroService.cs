using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Ekinci.CMS.Business.Services
{
    public class IntroService : BaseService, IIntroService
    {
        const string file = "Intro/";
        public IntroService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddIntro(AddIntroRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Intros.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta tanıtım zaten kayıtlıdır.");
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
            _context.Intros.Add(intro);
            await _context.SaveChangesAsync();
            result.SetSuccess("Tanıtım başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult<GetIntroResponse>> GetIntro()
        {
            var result = new ServiceResult<GetIntroResponse>();
            if (_context.Intros.Count() == 0)
            {
                result.SetError("Tanıtım yok");
                return result;
            }
            var Intro = await (from intro in _context.Intros
                               select new GetIntroResponse
                               {
                                   ID = intro.ID,
                                   Title = intro.Title,
                                   Description = intro.Description,
                                   PhotoUrl = ekinciUrl+intro.PhotoUrl,
                               }).FirstAsync();
            result.Data = Intro;
            return result;
        }

        public async Task<ServiceResult> UpdateIntro(UpdateIntroRequest request,IFormFile PhotoUrl)
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
                    result.SetError("Tanıtım kısmı Bulunamadı!");
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
                _context.Intros.Update(intro);
                await _context.SaveChangesAsync();
                result.SetSuccess("Tanıtım kısmı başarıyla güncellendi!");
            }
            else
            {
                result.SetError("Bu başlıkta tanıtım  zaten kayıtlıdır.");
            }

            return result;
        }
    }
}