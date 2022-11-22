using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class ContactController : Controller
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
            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetContact(int contactId)
        {
            var result = await contactService.GetContact(contactId);
            if (result.IsSuccess)
            {
                TempData["MessageText"] = result.Message;
                return View(result.Data);
            }
            TempData["MessageText"] = result.Message;
            return View();
        }
    }
}