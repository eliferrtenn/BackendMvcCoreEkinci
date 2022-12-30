using Ekinci.Common.Business;
using Ekinci.Common.Helpers;
using Ekinci.Common.JwtModels;
using Ekinci.Common.SMSSender;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.AccountRequests;
using Ekinci.WebAPI.Business.Models.Responses.AccountResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Ekinci.WebAPI.Business.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly ISMSSender smsSender;

        public AccountService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
        {
        }

        public async Task<ServiceResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = new ServiceResult<LoginResponse> { Data = new LoginResponse() };
            var member = await _context.Members.FirstOrDefaultAsync(x => x.MobilePhone == request.MobilePhone);
            if (member == null)
            {
                member = new Member
                {
                    MobilePhone = request.MobilePhone,
                    IsDeleted = false,
                    IsEnabled = false,
                    CreatedDate = DateTime.Now
                };
                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();
                result.Data.IsNewUser = true;
            }
            else if (member.IsEnabled)
            {
                result.Data.IsNewUser = false;
            }
            else
            {
                result.Data.IsNewUser = false;
            }
            var smsCode = string.Empty;
            if (request.MobilePhone == "909090909090" || request.MobilePhone == "905070033286") // Sabit numarayla giriş yapmak için Bu numara IOS ve Android ekibine verilecek.
            {
                smsCode = "2020";
            }
            else
            {
                smsCode = KeyGenerator.CreateRandomNumber(1000, 9999).ToString();

                var smsText = _localizer["SmsVerificationText"] + smsCode;
               // var smsResult = await smsSender.SendAsync(request.MobilePhone, smsText);
            }
            smsCode = "2020";
            member.SmsCode = smsCode.ToString();
            member.SmsCodeExpireDate = DateTime.Now.AddMinutes(3);
            var result2 = _context.Members.Update(member);
            await _context.SaveChangesAsync();

            result.SetSuccess("Sms gönderildi.");
            return result;
        }

        public async Task<ServiceResult<Token>> LoginSmsVerification(LoginSmsVerificationRequest request)
        {
            var result = new ServiceResult<Token>();
            var member = await _context.Members.FirstOrDefaultAsync(x => x.MobilePhone == request.MobilePhone);
            if (member == null)
            {
                result.SetError("Kullanıcı bulunamadı.");
                return result;
            }

            if (member.SmsCode != request.SmsCode)
            {
                result.SetError("Sms kodu yanlış.");
                return result;
            }
            if (DateTime.Now > member.SmsCodeExpireDate)
            {
                result.SetError("Süre geçti..");
                return result;
            }

            CustomTokenHandler tokenHandler = new(_configuration);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.ID.ToString()),
                new Claim(ClaimTypes.Name, member.FullName),
                new Claim(ClaimTypes.MobilePhone, member.MobilePhone)
             };
            var token = tokenHandler.CreateAccessToken(claims);
            result.Data = token;
            return result;
        }
    }
}