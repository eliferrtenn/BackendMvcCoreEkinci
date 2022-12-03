using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.PressRequests;
using Ekinci.CMS.Business.Models.Requests.PressResponses;
using Ekinci.CMS.Business.Models.Responses.PressResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class PressService : BaseService, IPressService
    {
        public PressService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddPress(AddPressRequest request)
        {
            var result = new ServiceResult();
            //TODO:Buraya bir daha bak
            var press = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            //TODO:Basın alanına fotourl ekleme
            _context.Press.Add(press);
            await _context.SaveChangesAsync();

            result.SetSuccess("Basın fotoğrafı başarıyla başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> DeletePress(DeletePressRequest request)
        {
            var result = new ServiceResult();
            var press = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (press == null)
            {
                result.SetError("Basın fotoğrafı Bulunamadı!");
                return result;
            }

            press.IsEnabled = true;
            _context.Press.Update(press);
            await _context.SaveChangesAsync();
            result.SetSuccess("Basın fotoğrafı başarıyla alan silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListPressesResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListPressesResponse>>();
            var presses = await(from press in _context.Press
                                select new ListPressesResponse
                                {
                                    ID = press.ID,
                                    PhotoUrl = press.PhotoUrl,
                                    //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                }).ToListAsync();
            result.Data = presses;
            return result;
        }

        public async Task<ServiceResult<GetPressResponse>> GetPress(int PressID)
        {
            var result = new ServiceResult<GetPressResponse>();
            var press = await (from pres in _context.Press
                               where pres.ID == PressID
                               select new GetPressResponse
                               {
                                   ID = pres.ID,
                                   PhotoUrl = pres.PhotoUrl,
                                   //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                               }).FirstAsync();
            if (press == null)
            {
                result.SetError("Bu bölüm bulunamadı");
                return result;
            }
            result.Data = press;
            return result;
        }

        public async Task<ServiceResult> UpdatePress(UpdatePressRequest request)
        {
            var result = new ServiceResult();
            var press = await _context.Press.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (press == null)
            {
                result.SetError("Basın fotoğrafı Bulunamadı!");
                return result;
            }
            //TODO:Basına fotourl ekleme
            _context.Press.Update(press);
            await _context.SaveChangesAsync();
            result.SetSuccess("Basın fotoğrafı başarıyla güncellendi!");
            return result;
        }
    }
}
