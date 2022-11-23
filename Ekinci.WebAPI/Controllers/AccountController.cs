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

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await accountService.Login(request);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> LoginSmsVerification(LoginSmsVerificationRequest request)
        {
            var result = await accountService.LoginSmsVerification(request);
            return Ok(result);
        }

    }
}