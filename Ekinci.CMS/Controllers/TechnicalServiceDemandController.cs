using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.Common.BaseController;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ekinci.CMS.Controllers
{
    public class TechnicalServiceDemandController : CMSBaseController
    {
        private readonly ITechnicalServiceDemandService technicalServiceDemandService;

        public TechnicalServiceDemandController(ITechnicalServiceDemandService _technicalServiceDemandService)
        {
            technicalServiceDemandService = _technicalServiceDemandService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await technicalServiceDemandService.GetAll();
            return View(result.Data);
        }


        public async Task<IActionResult> Details(int id)
        {
            var result = await technicalServiceDemandService.GetTechnicalServiceDemand(id);
            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await technicalServiceDemandService.AssignPersonelTechnicalServiceDemand(id);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AssignPersonelTechnicalServiceDemandRequest request)
        {
            var result = await technicalServiceDemandService.AssignPersonelTechnicalServiceDemand(request);
            if (result.IsSuccess)
            {
                Message(result);
                return RedirectToAction("Index");
            }
            Message(result);
            return View();
        }
    }
}
