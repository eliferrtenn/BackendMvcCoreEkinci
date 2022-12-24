using Ekinci.Data.Models;

namespace Ekinci.CMS.Business.Models.Requests.AppSettingsRequest
{
    public class UpdateAppSettingsRequest
    {
        public List<AppSettings> AppSettingsList { get; set; }
    }
}