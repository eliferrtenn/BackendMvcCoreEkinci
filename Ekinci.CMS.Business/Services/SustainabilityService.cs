using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.SustainabilityRequests;
using Ekinci.CMS.Business.Models.Responses.SustainabilityResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class SustainabilityService : BaseService, ISustainabilityService
    {
        const string file = "Sustainability/";
        public SustainabilityService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddSustainability(AddSustainabilityRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Sustainabilities.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta sürdürülebilirlik zaten kayıtlıdır.");
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var sustainability = new Sustainability();
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
                    sustainability.PhotoUrl = type;
                }
            }
            sustainability.Title = request.Title;
            sustainability.Description = request.Description;
            _context.Sustainabilities.Add(sustainability);
            await _context.SaveChangesAsync();
            result.SetSuccess("Tarihçe başarıyla eklendi!");
            return result;
        }
        public async Task<ServiceResult<GetSustainabilityResponse>> GetSustainability()
        {
            var result = new ServiceResult<GetSustainabilityResponse>();

            if (_context.Sustainabilities.Count() == 0)
            {
                result.SetError("Sürdürülebilirlik bulunamadı");
                return result;
            }

            var sustainability = await (from sustain in _context.Sustainabilities
                                        where sustain.IsEnabled==true
                                        select new GetSustainabilityResponse
                                        {
                                            ID = sustain.ID,
                                            Title = sustain.Title,
                                            Description = sustain.Description,
                                            PhotoUrl = ekinciUrl + sustain.PhotoUrl,
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
                    result.SetError("Sürdürülebilirlik Bulunamadı!");
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + sustainability.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        sustainability.PhotoUrl = type;
                    }
                }
                sustainability.Title = request.Title;
                sustainability.Description = request.Description;
                _context.Sustainabilities.Update(sustainability);
                await _context.SaveChangesAsync();
                result.SetSuccess("Sürdürülebilirlik başarıyla güncellendi!");
            }
            return result;
        }

    }
}