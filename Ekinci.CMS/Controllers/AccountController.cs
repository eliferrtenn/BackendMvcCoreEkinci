using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AccountRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.CMS.Controllers
{
    public class AccountController : CMSBaseController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginRequest request)
        {
            var result = await accountService.SignIn(request);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            Message(result);
            return View();
        }

        public async Task<IActionResult> MyProfile()
        {
            var result = await accountService.GetProfile();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request, IFormFile ProfilePhotoUrl)
        {
            var result = await accountService.UpdateProfile(request, ProfilePhotoUrl);
            Message(result);
            return RedirectToAction("MyProfile","Account");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = await accountService.ForgotPassword(request);
            Message(result);
            return View();
        }     



        [AllowAnonymous]
        public async Task<IActionResult> Sign_Out()
        {
            await accountService.SignOut();
            return RedirectToAction(nameof(SignIn));
        }
    }
}