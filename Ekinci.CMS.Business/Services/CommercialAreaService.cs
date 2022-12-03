using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.CommercialAreaRequests;
using Ekinci.CMS.Business.Models.Responses.CommercialAreaResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class CommercialAreaService : BaseService, ICommercialAreaService
    {
        public CommercialAreaService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddCommercialArea(AddCommercialAreaRequest request)
        {
            var result = new ServiceResult();
            var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (commercialArea != null)
            {
                result.SetError("Bu isimde Ticari Alan zaten kayıtlıdır.");
                return result;
            }
            commercialArea!.Title = request.Title;
            //TODO:Ticari alana fotourl ekleme
            _context.CommercialAreas.Add(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess("Ticari Alan başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> DeleteCommercialArea(DeleteCommercialAreaRequest request)
        {
            var result = new ServiceResult();
            var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (commercialArea == null)
            {
                result.SetError("Ticari Alan Bulunamadı!");
                return result;
            }

            commercialArea.IsEnabled = true;
            _context.CommercialAreas.Update(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess("Ticari alan silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListCommercialAreasResponse>>();
            var commercials = await (from commercial in _context.CommercialAreas
                                     select new ListCommercialAreasResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl,
                                         //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
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
                                         PhotoUrl = commercial.PhotoUrl,
                                         //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError("Ticari alan bulunamadı");
                return result;
            }
            result.Data = commercials;
            return result;
        }


        public async Task<ServiceResult> UpdateCommercialArea(UpdateCommercialAreaRequest request)
        {
            var result = new ServiceResult();
            var commercialArea = await _context.CommercialAreas.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (commercialArea == null)
            {
                result.SetError("Ticari alan Bulunamadı!");
                return result;
            }
            commercialArea.Title = request.Title;
            //TODO:Ticari alana fotourl ekleme
            _context.CommercialAreas.Update(commercialArea);
            await _context.SaveChangesAsync();

            result.SetSuccess("Ticari Alan başarıyla güncellendi!");
            return result;
        }
    }
}