using Ekinci.CMS.Business.Models.Requests.ContactRequests;
using Ekinci.CMS.Business.Models.Responses.ContactResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IContactService
    {
        Task<ServiceResult> AddContact(AddContactRequest request);
        Task<ServiceResult> UpdateContact(UpdateContactRequest request);
        Task<ServiceResult> DeleteContact(DeleteContactRequest request);
        Task<ServiceResult<List<ListContactsResponse>>> GetAll();
        Task<ServiceResult<GetContactResponse>> GetContact(int ContactID);
    }
}