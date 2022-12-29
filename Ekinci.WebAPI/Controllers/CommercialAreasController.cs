using Ekinci.Common.BaseController;
using Ekinci.WebAPI.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.WebAPI.Controllers
{
    public class CommercialAreasController : APIBaseController
    {
        private readonly ICommercialAreaService commercialAreaService;

        public CommercialAreasController(ICommercialAreaService CommercialAreaService)
        {
            commercialAreaService = CommercialAreaService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await commercialAreaService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommercialArea(int CommercialAreaID)
        {
            var result = await commercialAreaService.GetCommercialArea(CommercialAreaID);
            return Ok(result);
        }
    
    }
}