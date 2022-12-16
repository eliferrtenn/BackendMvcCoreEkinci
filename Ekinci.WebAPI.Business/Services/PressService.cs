using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.PressResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class PressService : BaseService, IPressService
    {
        public PressService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListPressResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListPressResponse>>();
            var presses = await (from press in _context.Press
                                 select new ListPressResponse
                                 {
                                     ID = press.ID,
                                     PhotoUrl = ekinciUrl + press.PhotoUrl,
                                 }).ToListAsync();
            result.Data = presses;
            return result;
        }

        public async Task<ServiceResult<GetPressResponse>> GetPress(int pressID)
        {
            var result = new ServiceResult<GetPressResponse>();
            var press = await (from pres in _context.Press
                               where pres.ID == pressID
                               select new GetPressResponse
                               {
                                   ID = pres.ID,
                                   PhotoUrl = ekinciUrl + pres.PhotoUrl,
                               }).FirstAsync();
            if (press == null)
            {
                result.SetError("Bu bölüm bulunamadı");
                return result;
            }
            result.Data = press;
            return result;
        }
    }
}