using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.KvkRequests;
using Ekinci.CMS.Business.Models.Responses.KvkkResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.CMS.Business.Services
{
    public class KvkkService : BaseService, IKvkkService
    {
        public KvkkService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetKvkkResponse>> GetKvkk()
        {
            var result = new ServiceResult<GetKvkkResponse>();
            var kvkk = await(from kvk in _context.Kvkks
                             select new GetKvkkResponse
                             {
                                 ID = kvk.ID,
                                 Title = kvk.Title,
                                 Description = kvk.Description,
                                 PhotoUrl = kvk.PhotoUrl,
                                 //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                             }).FirstAsync();
            result.Data = kvkk;
            return result;
        }

        public async Task<ServiceResult> UpdateKvkk(UpdateKvkkResponse request)
        {
            var result = new ServiceResult();
            var kvkk = await _context.Kvkks.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (kvkk == null)
            {
                result.SetError("Kvkk Bulunamadı!");
                return result;
            }

            kvkk.Title = request.Title;
            kvkk.Description = request.Description;
            //TODO:Kvkk fotoğrafı güncelleme işlemi yapılacak
            _context.Kvkks.Update(kvkk);
            await _context.SaveChangesAsync();
            result.SetSuccess("Kvkk başarıyla güncellendi!");
            return result;
        }
    }
}
