using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class MemberController : APIBaseController
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
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMember(UpdateMemberRequest request)
        {

            var result = await memberService.UpdateMember(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember()
        {
            var result = await memberService.DeleteMember();
            return Ok(result);
        }
    }
}