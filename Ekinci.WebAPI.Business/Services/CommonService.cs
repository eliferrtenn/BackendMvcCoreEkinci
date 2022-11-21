using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.BlogResponses;
using Ekinci.WebAPI.Business.Models.Responses.HumanResourceResponse;
using Ekinci.WebAPI.Business.Models.Responses.IdentityGuideResponse;
using Ekinci.WebAPI.Business.Models.Responses.IntroResponse;
using Ekinci.WebAPI.Business.Models.Responses.KvkkResponse;
using Ekinci.WebAPI.Business.Models.Responses.SustainabilityResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class CommonService : BaseService, ICommonService
    {
        public CommonService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog()
        {
            var result = new ServiceResult<GetBlogResponse>();
            var blog = await (from blo in _context.Blog
                              select new GetBlogResponse
                              {
                                  ID = blo.ID,
                                  Title = blo.Title,
                                  BlogDate = blo.BlogDate,
                                  PhotoUrl = blo.PhotoUrl,
                                  //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                              }).FirstAsync();

            result.Data = blog;
            return result;
        }

        public async Task<ServiceResult<GetHumanResourceResponse>> GetHumanResorce()
        {
            var result = new ServiceResult<GetHumanResourceResponse>();
            var humanResource = await (from human in _context.HumanResources
                                       select new GetHumanResourceResponse
                                       {
                                           ID = human.ID,
                                           Title = human.Title,
                                           Description = human.Description,
                                       }).FirstAsync();
            result.Data = humanResource;
            return result;
        }

        public async Task<ServiceResult<GetIdentityGuideResponse>> GetIdentityGuide()
        {
            var result = new ServiceResult<GetIdentityGuideResponse>();
            var IdentityGuide = await (from identity in _context.IdentityGuides
                                       select new GetIdentityGuideResponse
                                       {
                                           ID = identity.ID,
                                           Title = identity.Title,
                                           PhotoUrl = identity.PhotoUrl,
                                           //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                       }).FirstAsync();
            result.Data = IdentityGuide;
            return result;
        }

        public async Task<ServiceResult<GetIntroResponse>> GetIntro()
        {
            var result = new ServiceResult<GetIntroResponse>();
            var Intro = await (from intro in _context.Intros
                               select new GetIntroResponse
                               {
                                   ID = intro.ID,
                                   Title = intro.Title,
                                   Description = intro.Description,
                                   PhotoUrl = intro.PhotoUrl,
                                   //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                               }).FirstAsync();
            result.Data = Intro;
            return result;
        }

        public async Task<ServiceResult<GetKvkkResponse>> GetKvkk()
        {
            var result = new ServiceResult<GetKvkkResponse>();
            var kvkk = await (from kvk in _context.Kvkks
                              select new GetKvkkResponse
                              {
                                  ID = kvk.ID,
                                  Title = kvk.Title,
                                  Description = kvk.Description,
                                  PhotoUrl = kvk.PhotoUrl,
                                  //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                              }).FirstAsync();
            result.Data = kvkk;
            return result;
        }

        public async Task<ServiceResult<GetSustainabilityResponse>> GetSustainability()
        {
            var result = new ServiceResult<GetSustainabilityResponse>();
            var sustainability = await (from sustain in _context.Sustainabilities
                                        select new GetSustainabilityResponse
                                        {
                                            ID = sustain.ID,
                                            Title = sustain.Title,
                                            Description = sustain.Description,
                                            PhotoUrl = sustain.PhotoUrl,
                                            //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                        }).FirstAsync();
            result.Data = sustainability;
            return result;
        }
    }
}