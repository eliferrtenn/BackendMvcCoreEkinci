using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HumanResourceRequests;
using Ekinci.CMS.Business.Models.Responses.HumanResourceResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class HumanResourceService : BaseService, IHumanResourceService
    {
        const string file = "HumanResource/";
        public HumanResourceService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddHumanResource(AddHumanResourceRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.HumanResources.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta insan kaynakları zaten kayıtlıdır.");
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var human = new HumanResource();
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
                    human.PhotoUrl = type;
                }
            }
            human.Title = request.Title;
            human.Description = request.Description;
            _context.HumanResources.Add(human);
            await _context.SaveChangesAsync();

            result.SetSuccess("İnsan Kaynakları başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult<GetHumanResourceResponse>> GetHumanResource()
        {
            var result = new ServiceResult<GetHumanResourceResponse>();
            if (_context.HumanResources.Count() == 0)
            {
                result.SetError("İnsan kaynakları bulunamadı");
                return result;
            }
            var humanResource = await (from human in _context.HumanResources
                                       select new GetHumanResourceResponse
                                       {
                                           ID = human.ID,
                                           Title = human.Title,
                                           Description = human.Description,
                                           PhotoUrl = ekinciUrl + human.PhotoUrl,
                                       }).FirstAsync();
            result.Data = humanResource;
            return result;
        }

        public async Task<ServiceResult> UpdateHumanResource(UpdateHumanResourceRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Histories.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var humanResource = await _context.HumanResources.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (humanResource == null)
                {
                    result.SetError("İnsan Kaynakları Bulunamadı!");
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + humanResource.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        humanResource.PhotoUrl = type;
                    }
                }
                humanResource.Title = request.Title;
                humanResource.Description = request.Description;
                _context.HumanResources.Update(humanResource);
                await _context.SaveChangesAsync();
                result.SetSuccess("İnsan Kaynakları başarıyla güncellendi!");
            }
            else
            {
                result.SetError("Bu başlıkta İnsan Kaynakları zaten kayıtlıdır.");
            }
            return result;
        }
    }
}