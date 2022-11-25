using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Ekinci.WebAPI.Business.ServicesCollections
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<EkinciContext>(ServiceLifetime.Transient);
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<IAnnouncementService, AnnouncementService>();
            serviceCollection.AddScoped<ICommercialAreaService, CommercialAreaService>();
            serviceCollection.AddScoped<ICommonService, CommonService>();
            serviceCollection.AddScoped<IContactService, ContactService>();
            serviceCollection.AddScoped<IHistoryService, HistoryService>();
            serviceCollection.AddScoped<IMemberService, MemberService>();
            serviceCollection.AddScoped<IPressService, PressService>();
            serviceCollection.AddScoped<IProjectsService, ProjectsService>();
            serviceCollection.AddScoped<IVideosService, VideosService>();
            return serviceCollection;
        }
    }
}
