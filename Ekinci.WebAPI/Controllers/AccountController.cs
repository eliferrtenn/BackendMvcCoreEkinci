using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.AccountRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService AccountService)
        {
            accountService = AccountService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.Login(request);
                TempData["MessageText"] = result.Message;
                return RedirectToAction("LoginSmsVerification");
            }
            return View();
        }

        [HttpGet]
        public IActionResult LoginSmsVerification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginSmsVerification(LoginSmsVerificationRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.LoginSmsVerification(request);
                if (result.IsSuccess == true)
                {
                    TempData["MessageText"] = result.Message;
                    return RedirectToAction("Home");
                }
                TempData["MessageText"] = result.Message;
            }
            return View();
        }

    }
}