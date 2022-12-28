using BunnyCDN.Net.Storage;
using Ekinci.Common.Caching;
using Ekinci.Common.Utilities.FtpUpload;
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
        protected AppSettingsKeys _appSettingsKeys;
        protected FileUpload _fileUpload;

        public BaseService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys,FileUpload fileUpload)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContext;
            _localizer = localizer;
            _appSettingsKeys = appSettingsKeys;
            _fileUpload = fileUpload;
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