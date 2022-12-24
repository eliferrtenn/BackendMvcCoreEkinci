using Ekinci.CMS.Business.Models.Requests.AppSettingsRequest;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IAppSettingsService
    {
        Task<ServiceResult> LoadAllSettingsToCache();
        Task<ServiceResult<UpdateAppSettingsRequest>> UpdateAsync();
        Task<ServiceResult<UpdateAppSettingsRequest>> UpdateAsync(UpdateAppSettingsRequest request);
    }
}