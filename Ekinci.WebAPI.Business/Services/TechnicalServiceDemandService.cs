using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.WebAPI.Business.Models.Responses.TechnicalServiceDemandResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.WebAPI.Business.Services
{
    public class TechnicalServiceDemandService : BaseService, ITechnicalServiceDemandService
    {
        public TechnicalServiceDemandService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext, IStringLocalizer<CommonResource> localizer, FileUpload fileUpload) : base(context, configuration, httpContext, localizer, fileUpload)
        {
        }

        public async Task<ServiceResult> AddTechnicalServiceDemand(AddTechnicalServiceDemandRequest request)
        {
            var result = new ServiceResult();
            var technicalServiceDemand = new TechnicalServiceDemand();
            technicalServiceDemand.MemberID = CurrentUserID;
            technicalServiceDemand.Title = request.Title;
            technicalServiceDemand.Description = request.Description;
            technicalServiceDemand.DemandType = request.DemandType;
            technicalServiceDemand.DemandUrgencyStatus = request.DemandUrgencyStatus;
            technicalServiceDemand.SiteName = request.SiteName;
            technicalServiceDemand.ApartmentName = request.ApartmentName;
            technicalServiceDemand.ApartmentFloor = request.ApartmentFloor;
            technicalServiceDemand.ApartmentNo = request.ApartmentFloor;
            technicalServiceDemand.ContactInform = request.ContactInform;

            _context.TechnicalServiceDemands.Add(technicalServiceDemand);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["BlogAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteTechnicalServiceDemand(DeleteTechnicalServiceDemandRequest request)
        {
            var result = new ServiceResult();
            var technic = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (technic == null)
            {
                result.SetError(_localizer["BlogNotFound"]);
                return result;
            }
            technic.IsEnabled = false;
            _context.TechnicalServiceDemands.Update(technic);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["BlogDeleted"]);
            return result;
        }

        public async Task<ServiceResult> EditTechnicalServiceDemand(EditTechnicalServiceDemandRequest request)
        {
            var result = new ServiceResult();
            var technicalServiceDemand = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (technicalServiceDemand == null)
            {
                result.SetError(_localizer["ContactNotFound"]);
                return result;
            }
            technicalServiceDemand.MemberID = CurrentUserID;
            technicalServiceDemand.Title = request.Title;
            technicalServiceDemand.Description = request.Description;
            technicalServiceDemand.DemandType = request.DemandType;
            technicalServiceDemand.DemandUrgencyStatus = request.DemandUrgencyStatus;
            technicalServiceDemand.SiteName = request.SiteName;
            technicalServiceDemand.ApartmentName = request.ApartmentName;
            technicalServiceDemand.ApartmentFloor = request.ApartmentFloor;
            technicalServiceDemand.ApartmentNo = request.ApartmentFloor;
            technicalServiceDemand.ContactInform = request.ContactInform;
            _context.TechnicalServiceDemands.Update(technicalServiceDemand);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["ContactUpdated"]);
            return result;
        }

        public async Task<ServiceResult<List<ListTechnicalServiceDemandResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListTechnicalServiceDemandResponse>>();
            if (_context.Blog.Count() == 0)
            {
                result.SetError(_localizer["BlogNotFound"]);
                return result;
            }
            var technics = await(from technic in _context.TechnicalServiceDemands
                              where technic.IsEnabled == true && technic.MemberID==CurrentUserID
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
                                  CreateDayDemand = technic.CreateDayDemand.ToFormattedDate(),
                                  SolutionDayDemand = technic.SolutionDayDemand.ToFormattedDate(),
                              }).ToListAsync();

            result.Data = technics;
            return result;
        }

        public async Task<ServiceResult<GetTechnicalServiceDemandResponse>> GetTechnicalServiceDemand(int technicalDemandServiceID)
        {
            var result = new ServiceResult<GetTechnicalServiceDemandResponse>();
            var technics = await(from technic in _context.TechnicalServiceDemands
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
                                  CreateDayDemand = technic.CreateDayDemand.ToFormattedDate(),
                                  SolutionDayDemand = technic.SolutionDayDemand.ToFormattedDate(),
                              }).FirstAsync();
            if (technics == null)
            {
                result.SetError(_localizer["BlogNotFound"]);
                return result;
            }
            result.Data = technics;
            return result;
        }
    }
}
