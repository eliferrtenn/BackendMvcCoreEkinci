using Ekinci.Data.Context;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager _configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;


builder.Services.AddDbContext<EkinciContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

#region Core Services
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddHttpContextAccessor();
#endregion

#region Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<ICommercialAreaService, CommercialAreaService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPressService, PressService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<IVideosService, VideosService>();
#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EkinciAPP", Version = "v1" });
});

#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.OnStarting(() =>
            {
                context.Response.ContentType = "text/plain";
                return global::System.Threading.Tasks.Task.CompletedTask;
            });

            return Task.CompletedTask;
        }
    };

    opt.SaveToken = true;

    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _configuration["Token:Issuer"],
        ValidAudience = _configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"])),
        ClockSkew = TimeSpan.FromMinutes(5),
    };
});
#endregion
#region Localization
builder.Services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
