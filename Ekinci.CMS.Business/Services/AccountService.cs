using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AccountRequests;
using Ekinci.CMS.Business.Models.Responses.AccountResponses;
using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.CMS.Business.Services
{
    internal class AccountService : BaseService, IAccountService
    {
        public AccountService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = new ServiceResult<LoginResponse> { Data = new LoginResponse() };
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);
            if (user == null)
            {
                result.SetError("Giriş Başarısız.");
                return result;
            }
            result.SetSuccess("Giriş Başarılı.");
            return result;
        }
    }
}