using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class IdentityGuideController : Controller
    {
        private readonly IIdentityGuideService identityGuideService;

        public IdentityGuideController(IIdentityGuideService _identityGuideService)
        {
            identityGuideService = _identityGuideService;
        }

    }
}