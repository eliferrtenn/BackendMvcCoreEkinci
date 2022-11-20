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
