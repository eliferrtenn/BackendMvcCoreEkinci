using Ekinci.Common.Business;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.Common.BaseController
{
    public class CMSBaseController : Controller
    {
        public void Message(ServiceResult response)
        {
            if (response.Message != null)
            {
                if (response.IsSuccess)
                {
                    Success(response.Message);
                }
                else
                {
                    Error(response.Message);
                }
            }
        }

        public void Success(string message)
        {
            TempData["SucessMessage"] = message;
        }

        public void Warning(string message)
        {
            TempData["WarningMessage"] = message;
        }

        public void Error(string message)
        {
            TempData["ErrorMessage"] = message;
        }

    }
}