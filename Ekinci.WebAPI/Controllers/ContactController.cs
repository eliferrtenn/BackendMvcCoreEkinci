using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class ContactController : APIBaseController
    {
        private readonly IContactService contactService;

        public ContactController(IContactService ContactService)
        {
            contactService = ContactService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await contactService.GetAll();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetContact(int contactId)
        {
            var result = await contactService.GetContact(contactId);
            return Ok(result);
        }
    }
}