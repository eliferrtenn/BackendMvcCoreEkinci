using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.UserRequests;
using Ekinci.CMS.Business.Models.Responses.UserResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
        }

        public async Task<ServiceResult<GetUserResponse>> GetUser()
        {
            var result = new ServiceResult<GetUserResponse>();
            var member = await (from mem in _context.Users
                                where mem.ID == CurrentUserID
                                select new GetUserResponse
                                {
                                    ID = mem.ID,
                                    Firstname = mem.Firstname,
                                    Lastname = mem.Lastname,
                                    Email = mem.Email,
                                    MobilePhone = mem.MobilePhone,
                                    Password = mem.Password,
                                    //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                }).FirstAsync();

            result.Data = member;
            return result;
        }

        public async Task<ServiceResult> UpdateUser(UpdateUserRequest request)
        {
            var result = new ServiceResult();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ID == CurrentUserID);
            if (user == null)
            {
                result.SetError("Kullanıcı Bulunamadı!");
                return result;
            }

            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.Email = request.Email;
            user.MobilePhone = request.MobilePhone;
            user.Password = request.Password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            result.SetSuccess("Kullanıcı başarıyla güncellendi!");
            return result;
        }
    }
}