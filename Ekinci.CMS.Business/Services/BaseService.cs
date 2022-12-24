using BunnyCDN.Net.Storage;
using Ekinci.Common.Caching;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Ekinci.CMS.Business.Services
{
    public class BaseService
    {
        protected EkinciContext _context;
        protected IConfiguration _configuration;
        protected IHttpContextAccessor _httpContext;
        protected IStringLocalizer<CommonResource> _localizer;
        protected BunnyCDNStorage bunnyCDNStorage = new BunnyCDNStorage("ekinci", "257e5f3c-55fc-40b8-b00f2a941162-b427-4e2d", "de");
        protected const string ekinciUrl = "https://ekinci.b-cdn.net/";
        protected AppSettingsKeys _appSettingsKeys;
        public BaseService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContext;
            _localizer = localizer;
            _appSettingsKeys = appSettingsKeys;
        }

        public int CurrentUserID
        {
            get
            {
                return int.Parse(_httpContext.HttpContext.User.Identities.FirstOrDefault(u => u.IsAuthenticated && u.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))?.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }
        public int GetCurrentLanguageID(IStringLocalizer<CommonResource> localizer)
        {
            return int.Parse(localizer["Current_Language_ID"]);
        }
    }
}