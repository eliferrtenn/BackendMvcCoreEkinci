using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Resources;
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
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class CommonService : BaseService, ICommonService
    {
        const string fileHumanResource = "HumanResource/";
        const string fileIdentity = "IdentityGuide/";
        const string fileIntro = "Intro/";
        const string fileKvkk = "Kvkk/";
        const string fileSustain = "Sustainability/";

        public CommonService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer) : base(context, configuration, httpContext, localizer)
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
                                  BlogDate = blo.BlogDate.ToFormattedDate(),
                                  PhotoUrl = blo.PhotoUrl,
                                  InstagramUrl = blo.InstagramUrl,
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
                                           PhotoUrl=human.PhotoUrl,
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
                                           PhotoUrl = identity.PhotoUrl.PrepareCDNUrl(fileIdentity),
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
                                   SquareMeter= intro.SquareMeter,
                                   YearCount= intro.YearCount,
                                   CommercialAreaCount= intro.CommercialAreaCount,
                                   PhotoUrl = intro.PhotoUrl.PrepareCDNUrl(fileIntro),
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
                                  PhotoUrl = kvk.PhotoUrl.PrepareCDNUrl(fileKvkk),
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
                                            PhotoUrl = sustain.PhotoUrl.PrepareCDNUrl(fileSustain),
                                        }).FirstAsync();
            result.Data = sustainability;
            return result;
        }

    }
}