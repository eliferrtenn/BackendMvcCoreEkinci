﻿using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceDemandRequests;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceDemandResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.SMSSender;
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
        private readonly ISMSSender smsSender;
        public TechnicalServiceDemandService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload,ISMSSender _smsSender) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
            smsSender = _smsSender;
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
                                                where technic.IsEnabled == true && technic.TechnicalServiceStaffID.ToString() != null
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
                                                where technic.IsEnabled == true && technic.TechnicalServiceStaffID.ToString() == null
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
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).ToListAsync();

            result.Data = technicalServiceDemand;
            return result;
        }

        public async Task<ServiceResult> AssignPersonelTechnicalServiceDemand(AssignPersonelTechnicalServiceDemandRequest request)
        {
            var result = new ServiceResult();
            var technicalServiceDemand = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (technicalServiceDemand == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            technicalServiceDemand.TechnicalServiceStaffID = request.ID;
            //TODO:Personele mesaj gönderme işlemi
            var smsText = "Başlık :"+technicalServiceDemand.Title+"\nTanımı : "+technicalServiceDemand.Description+
                "\nAciliyet Durumu : "+technicalServiceDemand.DemandUrgencyStatus+"\nSite Adı : "+technicalServiceDemand.SiteName+
                "\nApartman Adı : "+technicalServiceDemand.ApartmentName+"\nKaçıncı katta : "+technicalServiceDemand.ApartmentFloor+
                "\nBina No : "+technicalServiceDemand.ApartmentNo+"\nİletişim Bilgileri : "+technicalServiceDemand.ContactInform;

            var technicalService = await _context.TechnicalServiceStaffs.FirstOrDefaultAsync(x => x.ID == technicalServiceDemand.TechnicalServiceStaffID);
            var smsResult = await smsSender.SendAsync(technicalService.ID.ToString(), smsText);
            _context.TechnicalServiceDemands.Update(technicalServiceDemand);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["TechnicalServiceDemandSendPersonel"]);
            return result;
        }

        public async Task<ServiceResult<GetTechnicalServiceDemandResponse>> GetTechnicalServiceDemand(int technicalDemandServiceID)
        {
            var result = new ServiceResult<GetTechnicalServiceDemandResponse>();
            var technicServiceStaff = _context.TechnicalServiceStaffs;
            var technicalService = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == technicalDemandServiceID);

            int count = 0;
            foreach (var i in technicServiceStaff)
            {
                if (technicalService.TechnicalServiceStaffID == i.ID)
                {
                    count++;
                }
            }

            if (count != 0)
            {
                var technicalServiceDemandd = await (from technic in _context.TechnicalServiceDemands
                                                    where technic.ID == technicalDemandServiceID && technic.IsEnabled == true
                                                    join ps in _context.TechnicalServiceStaffs on technic.TechnicalServiceStaffID equals ps.ID
                                                    select new GetTechnicalServiceDemandResponse
                                                    {
                                                        ID = technic.ID,
                                                        TechnicalServiceStaffID = technic.TechnicalServiceStaffID != null ? (int)technic.TechnicalServiceStaffID : 0,
                                                        TechnicalServiceName = ps.FullName,
                                                        TechnicalServiceMobilePhone = ps.MobilePhone,
                                                        DemandType = technic.DemandType,
                                                        Title = technic.Title,
                                                        Description = technic.Description,
                                                        DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                        SiteName = technic.SiteName,
                                                        ApartmentName = technic.ApartmentName,
                                                        ApartmentNo = technic.ApartmentNo,
                                                        ContactInform = technic.ContactInform,
                                                        CreateDayDemand = technic.CreateDayDemand,
                                                        SolutionDayDemand = technic.SolutionDayDemand,
                                                    }).FirstOrDefaultAsync();
                if (technicalServiceDemandd == null)
                {
                    result.SetError(_localizer["RecordNotFound"]);
                    return result;
                }
                result.Data = technicalServiceDemandd;
                return result;
            }
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.ID == technicalDemandServiceID && technic.IsEnabled == true
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
                                                    CreateDayDemand = technic.CreateDayDemand,
                                                    SolutionDayDemand = technic.SolutionDayDemand,
                                                }).FirstOrDefaultAsync();

            if (technicalServiceDemand == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = technicalServiceDemand;
            return result;
        }

        public async Task<ServiceResult<AssignPersonelTechnicalServiceDemandRequest>> AssignPersonelTechnicalServiceDemand(int technicalDemandServiceID)
        {
            var result = new ServiceResult<AssignPersonelTechnicalServiceDemandRequest>();

            var technicServiceStaff = _context.TechnicalServiceStaffs;
            var technicalService = await _context.TechnicalServiceDemands.FirstOrDefaultAsync(x => x.ID == technicalDemandServiceID);

            int count = 0;
            foreach (var i in technicServiceStaff)
            {
                if (technicalService.TechnicalServiceStaffID == i.ID)
                {
                    count++;
                }
            }

            if (count != 0)
            {
                var technicalServiceDemandd = await (from technic in _context.TechnicalServiceDemands
                                                     where technic.ID == technicalDemandServiceID && technic.IsEnabled == true
                                                     join ps in _context.TechnicalServiceStaffs on technic.TechnicalServiceStaffID equals ps.ID
                                                     select new AssignPersonelTechnicalServiceDemandRequest
                                                     {
                                                         ID = technic.ID,
                                                         TechnicalServiceStaffID = technic.TechnicalServiceStaffID != null ? (int)technic.TechnicalServiceStaffID : 0,
                                                         DemandType = technic.DemandType,
                                                         Title = technic.Title,
                                                         Description = technic.Description,
                                                         DemandUrgencyStatus = technic.DemandUrgencyStatus,
                                                         SiteName = technic.SiteName,
                                                         ApartmentName = technic.ApartmentName,
                                                         ApartmentNo = technic.ApartmentNo,
                                                         ContactInform = technic.ContactInform,
                                                         CreateDayDemand = technic.CreateDayDemand,
                                                         SolutionDayDemand = technic.SolutionDayDemand,
                                                     }).FirstOrDefaultAsync();
                if (technicalServiceDemandd == null)
                {
                    result.SetError(_localizer["RecordNotFound"]);
                    return result;
                }
                result.Data = technicalServiceDemandd;
                return result;
            }
            var technicalServiceDemand = await (from technic in _context.TechnicalServiceDemands
                                                where technic.ID == technicalDemandServiceID && technic.IsEnabled == true
                                                select new AssignPersonelTechnicalServiceDemandRequest
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