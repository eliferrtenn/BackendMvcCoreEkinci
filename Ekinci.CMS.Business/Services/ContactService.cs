using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ContactRequests;
using Ekinci.CMS.Business.Models.Responses.ContactResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class ContactService : BaseService, IContactService
    {
        public ContactService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult> AddContact(AddContactRequest request)
        {
            var result = new ServiceResult();
            var exist = await _context.Contacts.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError(_localizer["ContactWithNameAlreadyExist"]);
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

            result.SetSuccess(_localizer["ContactAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteContact(DeleteContactRequest request)
        {
            var result = new ServiceResult();
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (contact == null)
            {
                result.SetError(_localizer["ContactNotFound"]);
                return result;
            }

            contact.IsEnabled = false;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["ContactDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListContactsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListContactsResponse>>();
            if (_context.Contacts.Count() == 0)
            {
                result.SetError(_localizer["ContactNotFound"]);
                return result;
            }
            var contacts = await (from con in _context.Contacts
                                  where con.IsEnabled == true
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
                result.SetError(_localizer["ContactNotFound"]);
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
                result.SetError(_localizer["ContactNotFound"]);
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

            result.SetSuccess(_localizer["ContactUpdated"]);
            return result;
        }
    }
}