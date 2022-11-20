using Ekinci.Common.Business;
using Ekinci.WebAPI.Business.Models.Responses.ContactResponse;

namespace Ekinci.WebAPI.Business.Interfaces
{
    public interface IContactService
    {
        Task<ServiceResult<List<ListContactsResponse>>> GetAll();
        Task<ServiceResult<GetContactResponse>> GetContact(int contactID);
    }
}