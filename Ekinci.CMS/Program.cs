using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Services;
using Ekinci.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<EkinciContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

#region Core Services
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddHttpContextAccessor();
#endregion

#region Services
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
builder.Services.AddScoped<ISustainabilityService, SustainabilityService>(); 
builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped<IVideosService, VideosService>();
#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
