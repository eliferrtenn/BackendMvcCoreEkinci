using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Models.Responses.IntroResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.CMS.Business.Services
{
    public class IntroService : BaseService, IIntroService
    {
        public IntroService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetIntroResponse>> GetIntro()
        {
            var result = new ServiceResult<GetIntroResponse>();
            var Intro = await(from intro in _context.Intros
                              select new GetIntroResponse
                              {
                                  ID = intro.ID,
                                  Title = intro.Title,
                                  Description = intro.Description,
                                  PhotoUrl = intro.PhotoUrl,
                                  //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                              }).FirstAsync();
            result.Data = Intro;
            return result;
        }

        public async Task<ServiceResult> UpdateIntro(UpdateIntroRequest request)
        {
            var result = new ServiceResult();
            var intro = await _context.Intros.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (intro == null)
            {
                result.SetError("Tanıtım kısmı Bulunamadı!");
                return result;
            }

            intro.Title = request.Title;
            intro.Description = request.Description;
            //TODO:Intro fotoğrafı güncelleme işlemi yapılacak
            _context.Intros.Update(intro);
            await _context.SaveChangesAsync();
            result.SetSuccess("Tanıtım kısmı başarıyla güncellendi!");
            return result;
        }
    }
}
