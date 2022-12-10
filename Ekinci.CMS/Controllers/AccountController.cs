using Ekinci.CMS.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}