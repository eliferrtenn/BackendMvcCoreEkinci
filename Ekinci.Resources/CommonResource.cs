using Microsoft.Extensions.Localization;

namespace Ekinci.Resources
{
    public interface ICommonResource
    {

    }
    public class CommonResource : ICommonResource
    {
        private readonly IStringLocalizer _localizer;

        public CommonResource(IStringLocalizer<CommonResource> localizer)
        {
            _localizer = localizer;
        }

        public string this[string index]
        {
            get
            {
                return _localizer[index];
            }
        }
    }
}