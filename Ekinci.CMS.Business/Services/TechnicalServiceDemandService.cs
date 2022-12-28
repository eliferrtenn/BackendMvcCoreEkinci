using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceDemandResponses;
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
    public class TechnicalServiceDemandService : BaseService, ITechnicalServiceDemandService
    {
        public TechnicalServiceDemandService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult<List<ListTechnicalServiceDemandResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListTechnicalServiceDemandResponse>>();
            if (_context.TechnicalServiceDemands.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.IsEnabled == true
                                                select new ListTechnicalServiceDemandResponse
                                                {
                                                    ID = technic.ID,
                                                    DemandType = technic.DemandType,
                                                    Title = technic.Title,
                                                    Description = technic.Description,
                                                    DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                    SiteName = technic.SiteName,
                                                    ApartmentName = technic.ApartmentName,
                                                    ApartmentNo = technic.ApartmentNo,
                                                    ContactInform = technic.ContactInform,
                                                    FullName = technic.FullName,
                                                    MobilePhone = technic.MobilePhone,
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).ToListAsync();

            result.Data = technicalServiceDemand;
            return result;
        }

        public async Task<ServiceResult<List<ListAssignTechnicalServiceDemandResponse>>> ListAssignTechnicalServiceDemand()
        {
            var result = new ServiceResult<List<ListAssignTechnicalServiceDemandResponse>>();
            if (_context.TechnicalServiceDemands.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.IsEnabled == true && technic.FullName != null && technic.MobilePhone != null
                                                select new ListAssignTechnicalServiceDemandResponse
                                                {
                                                    ID = technic.ID,
                                                    DemandType = technic.DemandType,
                                                    Title = technic.Title,
                                                    Description = technic.Description,
                                                    DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                    SiteName = technic.SiteName,
                                                    ApartmentName = technic.ApartmentName,
                                                    ApartmentNo = technic.ApartmentNo,
                                                    ContactInform = technic.ContactInform,
                                                    FullName = technic.FullName,
                                                    MobilePhone = technic.MobilePhone,
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).ToListAsync();

            result.Data = technicalServiceDemand;
            return result;
        }

        public async Task<ServiceResult<List<ListNonAssignmentTechnicalServiceDemendResponse>>> ListNonAssignmentTechnicalServiceDemend()
        {
            var result = new ServiceResult<List<ListNonAssignmentTechnicalServiceDemendResponse>>();
            if (_context.TechnicalServiceDemands.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.IsEnabled == true && technic.FullName == null && technic.MobilePhone == null
                                                select new ListNonAssignmentTechnicalServiceDemendResponse
                                                {
                                                    ID = technic.ID,
                                                    DemandType = technic.DemandType,
                                                    Title = technic.Title,
                                                    Description = technic.Description,
                                                    DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                    SiteName = technic.SiteName,
                                                    ApartmentName = technic.ApartmentName,
                                                    ApartmentNo = technic.ApartmentNo,
                                                    ContactInform = technic.ContactInform,
                                                    FullName = technic.FullName,
                                                    MobilePhone = technic.MobilePhone,
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).ToListAsync();

            result.Data = technicalServiceDemand;
            return result;
        }

        public async Task<ServiceResult> AssignPersonelTechnicalServiceDemand(AssignPersonelTechnicalServiceDemandRequest request)
        {
            var result = new ServiceResult();
            var contact = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (contact == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            contact.FullName = request.FullName;
            contact.MobilePhone = request.MobilePhone;
            //TODO:Personele mesaj gönderme işlemi
            _context.TechnicalServiceDemands.Update(contact);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["TechnicalServiceDemandSendPersonel"]);
            return result;
        }

        public async Task<ServiceResult<GetTechnicalServiceDemandResponse>> GetTechnicalServiceDemand(int technicalDemandServiceID)
        {
            var result = new ServiceResult<GetTechnicalServiceDemandResponse>();
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.ID == technicalDemandServiceID
                                                select new GetTechnicalServiceDemandResponse
                                                {
                                                    ID = technic.ID,
                                                    DemandType = technic.DemandType,
                                                    Title = technic.Title,
                                                    Description = technic.Description,
                                                    DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                    SiteName = technic.SiteName,
                                                    ApartmentName = technic.ApartmentName,
                                                    ApartmentNo = technic.ApartmentNo,
                                                    ContactInform = technic.ContactInform,
                                                    FullName = technic.FullName,
                                                    MobilePhone = technic.MobilePhone,
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).FirstAsync();
            if (technicalServiceDemand == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = technicalServiceDemand;
            return result;
        }
    }
}