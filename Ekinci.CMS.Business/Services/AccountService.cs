using Ekinci.CMS.Business.Constants;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AccountRequests;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Helpers;
using Ekinci.Common.Utilities;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Ekinci.CMS.Business.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IMailService mailService;

        public AccountService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload,IMailService _mailService) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
            mailService = _mailService;
        }

        public async Task<ServiceResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = new ServiceResult();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email.Trim());

            if (user is null)
            {
                result.SetError(_localizer["UserNotFound"]);
                return result;
            }

            user.Password = KeyGenerator.CreateRandomPassword(6);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var emailParameters = new Dictionary<string, string>
                {
                    { "[UserFullName]", user.FullName},
                    { "[Email]", user.Email },
                    { "[MobilePhone]", user.MobilePhone.ToPhoneNumber() },
                    { "[Password]", user.Password },
                };

            var emailResult = await mailService.Send(
                user.Email,
                "Yeni Şifreniz Kullanıma Hazır",
                "PasswordUpdated",
                emailParameters);

            if (!emailResult.IsSuccess)
            {
                result.SetError(emailResult.Message);
                return result;
            }

            result.SetSuccess(_localizer["EmailSentForPassword"]);
            return result;
        }

        public async Task<ServiceResult<UpdateProfileRequest>> GetProfile()
        {
            var result = new ServiceResult<UpdateProfileRequest>();
            var user = await (from us in _context.Users
                              where us.ID == CurrentUserID
                              select new UpdateProfileRequest
                              {
                                  ID = us.ID,
                                  Firstname = us.Firstname,
                                  Lastname = us.Lastname,
                                  Email = us.Email,
                                  MobilePhone = us.MobilePhone,
                                  ProfilePhotoUrl = us.ProfilePhotoUrl,
                                  //TODO:PhotoUrl Yapılacak
                              }).FirstOrDefaultAsync();

            if (user is null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }

            result.Data = user;
            return result;
        }

        public async Task<ServiceResult> SignIn(LoginRequest request)
        {
            var result = new ServiceResult();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password);

            if (user == null)
            {
                result.SetError(_localizer["LoginFailed"]);
                return result;
            }


            //TODO: Photo URL var userPhotoUrl = user.ProfilePhotoUrl.PrepareCDNUrl(_appSettingsKeys.CDN_UsersImageFolder);
            //userPhotoUrl = string.IsNullOrEmpty(userPhotoUrl) ? string.Empty : userPhotoUrl;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.MobilePhone),
            };


            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieNames.AuthCookieName));
            await _httpContext.HttpContext.SignInAsync(
                CookieNames.AuthCookieName,
                userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1),
                    IsPersistent = true,
                    AllowRefresh = true
                });

            return result;
        }

        public async Task<ServiceResult> SignOut()
        {
            var result = new ServiceResult();
            await _httpContext.HttpContext.SignOutAsync();
            result.SetSuccess(_localizer["SignedOut"]);
            return result;
        }

        public async Task<ServiceResult> UpdateProfile(UpdateProfileRequest request,IFormFile ProfilePhotoUrl)
        {
            var result = new ServiceResult();
            var member = await _context.Users.FirstOrDefaultAsync(x => x.ID == CurrentUserID);
            if (member == null)
            {
                result.SetError(_localizer["UserNotFound"]);
                return result;
            }

            member.Firstname = request.Firstname;
            member.Lastname = request.Lastname;
            member.Email = request.Email;
            member.MobilePhone = request.MobilePhone;

            _context.Users.Update(member);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["UserUpdated"]);
            return result;
        }
    }
}