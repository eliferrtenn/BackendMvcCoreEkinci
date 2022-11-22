using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService MemberService)
        {
            memberService = MemberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMember()
        {
            var result = await memberService.GetMember();
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult UpdateMember()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMember(UpdateMemberRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await memberService.UpdateMember(request);
                if (!result.IsSuccess)
                {
                    TempData["MessageText"] = result.Message;
                    return View();
                }
                TempData["MessageText"] = result.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember()
        {
            if (ModelState.IsValid)
            {
                var result = await memberService.DeleteMember();
                if (!result.IsSuccess)
                {
                    TempData["MessageText"] = result.Message;
                    return View();
                }
                TempData["MessageText"] = result.Message;
            }
            return View();
        }
    }
}