using BunnyCDN.Net.Storage;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Ekinci.WebAPI.Business.Services
{
    public class BaseService
    {
        protected EkinciContext _context;
        protected IConfiguration _configuration;
        protected IHttpContextAccessor _httpContext;
        protected BunnyCDNStorage bunnyCDNStorage = new BunnyCDNStorage("ekinci", "257e5f3c-55fc-40b8-b00f2a941162-b427-4e2d", "de");
        protected const string ekinciUrl = "https://ekinci.b-cdn.net/";

        public BaseService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public int CurrentUserID
        {
            get
            {
                return int.Parse(_httpContext.HttpContext.User.Identities.FirstOrDefault(u => u.IsAuthenticated && u.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))?.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }
    }
}