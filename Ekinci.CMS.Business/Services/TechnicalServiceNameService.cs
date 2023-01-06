using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceNameResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class TechnicalServiceNameService : BaseService, ITechnicalServiceNameService
    {
        public TechnicalServiceNameService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult<List<ListTechnicalServiceNameResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListTechnicalServiceNameResponse>>();
            if (_context.ProjectStatus.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var names = await (from name in _context.TechnicalServiceNames
                               select new ListTechnicalServiceNameResponse
                               {
                                   ID = name.ID,
                                   Name = name.Name,
                               }).ToListAsync();
            result.Data = names;
            return result;
        }
    }
}