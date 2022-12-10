using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.ContactRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService contactService;

        public ContactController(IContactService _contactService)
        {
            contactService = _contactService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await contactService.GetAll();
            return View(result.Data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await contactService.GetContact(id);
            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddContactRequest request)
        {
            var result = await contactService.AddContact(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await contactService.GetContact(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateContactRequest request)
        {
            var result = await contactService.UpdateContact(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");

            return View(result.Message);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteContactRequest request)
        {
            var result = await contactService.DeleteContact(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}