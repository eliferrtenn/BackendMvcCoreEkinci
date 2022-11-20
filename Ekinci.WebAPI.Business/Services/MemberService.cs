using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.MemberRequests;
using Ekinci.WebAPI.Business.Models.Responses.MemberResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }       

        public async Task<ServiceResult<GetMemberResponse>> GetMember()
        {
            var result = new ServiceResult<GetMemberResponse>();

            var member = await (from mem in _context.Members
                                where mem.ID == CurrentUserID
                                select new GetMemberResponse
                                {
                                    ID = mem.ID,
                                    Firstname = mem.Firstname,
                                    Lastname = mem.Lastname,
                                    Email = mem.Email,
                                    ProfilePhotoUrl = mem.ProfilePhotoUrl
                                    //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                }).FirstAsync();

            result.Data = member;
            return result;
        }

        public async Task<ServiceResult> UpdateMember(UpdateMemberRequest request)
        {
            var result = new ServiceResult();
            var member = await _context.Members.FirstOrDefaultAsync(x => x.ID == CurrentUserID);
            if (member == null)
            {
                result.SetError("Kullanıcı Bulunamadı!");
                return result;
            }

            member.Firstname = request.Firstname;
            member.Lastname = request.Lastname;
            member.Email = request.Email;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();

            result.SetSuccess("Kullanıcı başarıyla güncellendi!");
            return result;
        }
        public async Task<ServiceResult> DeleteMember()
        {
            var result = new ServiceResult();
            var member = await _context.Members.FirstOrDefaultAsync(x => x.ID == CurrentUserID);
            if (member == null)
            {
                result.SetError("Kullanıcı Bulunamadı!");
                return result;
            }

            member.IsDeleted = true;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();

            result.SetSuccess("Kullanıcı silindi.");
            return result;
        }
    }
}