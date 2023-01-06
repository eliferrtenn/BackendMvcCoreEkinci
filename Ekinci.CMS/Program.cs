using Ekinci.CMS.Business.Constants;
using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models;
using Ekinci.CMS.Business.Services;
using Ekinci.Common.Caching;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<EkinciContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});
builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BaseValidator>());

#region Core Services
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICacheManager, CacheManager>();
builder.Services.AddScoped<AppSettingsKeys>();
builder.Services.AddScoped<Ekinci.Common.Caching.AppSettingsKeys>();
builder.Services.AddScoped<IMailService, MailManager>();
builder.Services.AddScoped<IFTPClient, FTPClient>();
builder.Services.AddScoped<FileUpload>();
builder.Services.AddFluentValidationAutoValidation();
#endregion


#region Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICommercialAreaService, CommercialAreaService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IHumanResourceService, HumanResourceService>();
builder.Services.AddScoped<IIdentityGuideService, IdentityGuideService>();
builder.Services.AddScoped<IIntroService, IntroService>();
builder.Services.AddScoped<IKvkkService, KvkkService>();
builder.Services.AddScoped<IPressService, PressService>(); 
builder.Services.AddScoped<IProjectService, ProjectService>(); 
builder.Services.AddScoped<IProjectStatusService, ProjectStatusService>(); 
builder.Services.AddScoped<ISustainabilityService, SustainabilityService>(); 
builder.Services.AddScoped<ITechnicalServiceDemandService, TechnicalServiceDemandService>(); 
builder.Services.AddScoped<ITechnicalServiceStaffService, TechnicalServiceStaffService>(); 
builder.Services.AddScoped<ITechnicalServiceNameService, TechnicalServiceNameService>(); 
builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped<IVideosService, VideosService>();
#endregion

#region Authentication
builder.Services.AddAuthentication(CookieNames.AuthCookieName)
    .AddCookie(CookieNames.AuthCookieName, options =>
    {
        options.LoginPath = new PathString("/Account/SignIn");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
    });
#endregion

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
#region Localization
builder.Services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
#endregion

// Add services to the container.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var appsettings = app.Services.CreateScope().ServiceProvider.GetRequiredService<IAppSettingsService>();
await appsettings.LoadAllSettingsToCache();

var appsettingsKeys = app.Services.CreateScope().ServiceProvider.GetRequiredService<Ekinci.Common.Caching.AppSettingsKeys>();
StringExtensions._appSettingsKeys = appsettingsKeys;
app.Run();
