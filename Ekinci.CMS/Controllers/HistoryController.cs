using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.HistoryRequests;
using Ekinci.CMS.Business.Models.Requests.IntroRequests;
using Ekinci.CMS.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Ekinci.CMS.Controllers
{
	public class HistoryController : Controller
	{
		private readonly IHistoryService historyService;

		public HistoryController(IHistoryService _historyService)
		{
			historyService = _historyService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await historyService.GetAll();
			return View(result.Data);
		}

		public async Task<IActionResult> Details(int id)
		{
			var result = await historyService.GetHistory(id);
			return View(result.Data);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(AddHistoryRequest request,IFormFile PhotoUrl)
		{
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            if (PhotoUrl != null)
			{
				if (PhotoUrl.Length > 0)
				{
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/History/" + type;
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    request.PhotoUrl = "Dosya/History/" + type;
                }
			}
			var result = await historyService.AddHistory(request);
			if (result.IsSuccess)
				return RedirectToAction("Index");

			return View(result.Message);
		}
		public async Task<IActionResult> Edit(int id)
		{
			var result = await historyService.GetHistory(id);
			return View(result.Data);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(UpdateHistoryRequest request,IFormFile PhotoUrl)
		{
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var resultObject = await historyService.GetHistory(request.ID);
            if (PhotoUrl == null)
            {
                if (resultObject.Data != null)
                {
                    request.PhotoUrl = resultObject.Data.PhotoUrl;
                }
			}
			else
			{
                var filePath = "wwwroot/" + resultObject.Data.PhotoUrl;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                var path = Path.GetExtension(PhotoUrl.FileName);
                var type = guid.ToString() + path;
                filePath = "wwwroot/Dosya/History/" + type;
                filePaths.Add(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoUrl.CopyToAsync(stream);
                }
                request.PhotoUrl = "Dosya/History/" + type;
            }

            var result = await historyService.UpdateHistory(request);
			if (result.IsSuccess)
				return RedirectToAction("Index");

			return View(result.Message);
		}
        [HttpPost]
        public async Task<JsonResult> Delete(DeleteHistoryRequest request)
        {
            var result = await historyService.DeleteHistory(request);
            var ajaxResult = JsonConvert.SerializeObject(result);
            return Json(ajaxResult);
        }
    }
}