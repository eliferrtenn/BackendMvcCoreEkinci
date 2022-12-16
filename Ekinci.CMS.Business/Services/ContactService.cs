using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ContactRequests;
using Ekinci.CMS.Business.Models.Responses.ContactResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    public class ContactService : BaseService, IContactService
    {
        public ContactService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult> AddContact(AddContactRequest request)
        {
            var result = new ServiceResult();
            var exist = await _context.Contacts.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu isimde iletişim bilgisi zaten kayıtlıdır.");
                return result;
            }
            var contact = new Contact();
            contact.Title = request.Title;
            contact.Location = request.Location;
            contact.MobilePhone = request.MobilePhone;
            contact.LandPhone = request.LandPhone;
            contact.Email = request.Email;
            contact.Latitude = request.Latitude;
            contact.Longitude = request.Longitude;
            contact.InstagramUrl = request.InstagramUrl;
            contact.FacebookUrl = request.FacebookUrl;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            result.SetSuccess("İletişim bilgileri başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> DeleteContact(DeleteContactRequest request)
        {
            var result = new ServiceResult();
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (contact == null)
            {
                result.SetError("İletişim bilgisi Bulunamadı!");
                return result;
            }

            contact.IsEnabled = false;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            result.SetSuccess("İletişim bilgisi silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListContactsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListContactsResponse>>();
            if (_context.Contacts.Count() == 0)
            {
                result.SetError("İletişim bulunamadı");
                return result;
            }
            var contacts = await (from con in _context.Contacts
                                  where con.IsEnabled==true
                                  select new ListContactsResponse
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

        public async Task<ServiceResult<GetContactResponse>> GetContact(int ContactID)
        {
            var result = new ServiceResult<GetContactResponse>();
            var contact = await (from con in _context.Contacts
                                 where con.ID == ContactID
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

        public async Task<ServiceResult> UpdateContact(UpdateContactRequest request)
        {
            var result = new ServiceResult();
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (contact == null)
            {
                result.SetError("İletişim bilgisi Bulunamadı!");
                return result;
            }
            contact.Title = request.Title;
            contact.Location = request.Location;
            contact.MobilePhone = request.MobilePhone;
            contact.LandPhone = request.LandPhone;
            contact.Email = request.Email;
            contact.Latitude = request.Latitude;
            contact.Longitude = request.Longitude;
            contact.InstagramUrl = request.InstagramUrl;
            contact.FacebookUrl = request.FacebookUrl;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            result.SetSuccess("İletişim bilgisi başarıyla güncellendi!");
            return result;
        }
    }
}