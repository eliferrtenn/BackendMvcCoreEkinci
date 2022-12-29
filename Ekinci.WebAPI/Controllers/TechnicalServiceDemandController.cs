using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.TechnicalServiceDemandRequests;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class TechnicalServiceDemandController : APIBaseController
    {
        private readonly ITechnicalServiceDemandService technicalServiceDemandService;

        public TechnicalServiceDemandController(ITechnicalServiceDemandService TechnicalServiceDemandService)
        {
            technicalServiceDemandService = TechnicalServiceDemandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTechnicalServiceDemandRequest request)
        {
            var result = await technicalServiceDemandService.AddTechnicalServiceDemand(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTechnicalServiceDemandRequest request)
        {
            var result = await technicalServiceDemandService.EditTechnicalServiceDemand(request);
            return Ok(result);
        }
  
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteTechnicalServiceDemandRequest request)
        {
            var result = await technicalServiceDemandService.DeleteTechnicalServiceDemand(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await technicalServiceDemandService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommercialArea(int TechnicalServiceID)
        {
            var result = await technicalServiceDemandService.GetTechnicalServiceDemand(TechnicalServiceID);
            return Ok(result);
        }
    }
}