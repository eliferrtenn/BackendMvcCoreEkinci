using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.ContactResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class ContactService : BaseService, IContactService
    {
        public ContactService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListContactResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListContactResponse>>();
            var contacts = await (from con in _context.Contacts
                                  select new ListContactResponse
                                  {
                                      ID = con.ID,
                                      Title = con.Title,
                                      Location = con.Location,
                                      MobilePhone = con.MobilePhone,
                                      LandPhone = con.LandPhone,
                                      Email = con.Email,
                                      Longitude = con.Longitude,
                                      Latitude = con.Latitude,
                                  }).ToListAsync();
            result.Data = contacts;
            return result;
        }

        public async Task<ServiceResult<GetContactResponse>> GetContact(int contactID)
        {
            var result = new ServiceResult<GetContactResponse>();
            var contact = await (from con in _context.Contacts
                                 where con.ID == contactID
                                 select new GetContactResponse
                                 {
                                     ID = con.ID,
                                     Title = con.Title,
                                     Location = con.Location,
                                     MobilePhone = con.MobilePhone,
                                     LandPhone = con.LandPhone,
                                     Email = con.Email,
                                     Longitude = con.Longitude,
                                     Latitude = con.Latitude,
                                 }).FirstAsync();
            if (contact == null)
            {
                result.SetError("İletişim bilgisi bulunamadı");
                return result;
            }
            result.Data = contact;
            return result;
        }
    }
}