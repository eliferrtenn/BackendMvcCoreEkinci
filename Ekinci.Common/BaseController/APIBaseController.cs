using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ekinci.Common.BaseController
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class APIBaseController : ControllerBase
    {
    }
}
