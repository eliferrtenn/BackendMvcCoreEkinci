using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.AppSettingsRequest;
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
    public class AppSettingService : BaseService, IAppSettingsService
    {
        private readonly ICacheManager cacheManager;

        public AppSettingService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, ICacheManager _cacheManager) : base(context, configuration, localizer, httpContext, appSettingsKeys)
        {
            cacheManager = _cacheManager;
        }

        public async Task<ServiceResult> LoadAllSettingsToCache()
        {
            var result = new ServiceResult();

            var appSettings = await (from ap in _context.AppSettings
                                     orderby ap.Order
                                     select ap).ToListAsync();

            foreach (var item in appSettings)
            {
                cacheManager.SetNeverRemove(item.Key, item.Value);
            }

            return result;
        }

        public async Task<ServiceResult<UpdateAppSettingsRequest>> UpdateAsync()
        {
            var result = new ServiceResult<UpdateAppSettingsRequest>();

            var appSettings = new UpdateAppSettingsRequest
            {
                AppSettingsList = await (from ap in _context.AppSettings
                                         orderby ap.Order
                                         select ap).ToListAsync()
            };

            result.Data = appSettings;
            return result;
        }
   
        public async Task<ServiceResult<UpdateAppSettingsRequest>> UpdateAsync(UpdateAppSettingsRequest request)
        {
            var result = new ServiceResult<UpdateAppSettingsRequest>();

            _context.AppSettings.UpdateRange(request.AppSettingsList);
            await _context.SaveChangesAsync();

            var appSettings = new UpdateAppSettingsRequest
            {
                AppSettingsList = await (from ap in _context.AppSettings
                                         orderby ap.Order
                                         select ap).ToListAsync()
            };
            await LoadAllSettingsToCache();
            result.Data = appSettings;

            result.SetSuccess(_localizer["RecordsUpdated"]);
            return result;
        }
    }
}