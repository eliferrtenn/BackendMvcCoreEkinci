using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.TechnicalServiceStaffRequests;
using Ekinci.CMS.Business.Models.Responses.TechnicalServiceStaffResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Caching;
using Ekinci.Common.Utilities.FtpUpload;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Ekinci.CMS.Business.Services
{
    public class TechnicalServiceStaffService : BaseService, ITechnicalServiceStaffService
    {
        public TechnicalServiceStaffService(EkinciContext context, IConfiguration configuration, IStringLocalizer<CommonResource> localizer, IHttpContextAccessor httpContext, AppSettingsKeys appSettingsKeys, FileUpload fileUpload) : base(context, configuration, localizer, httpContext, appSettingsKeys, fileUpload)
        {
        }

        public async Task<ServiceResult> AddTechnicalServiceStaff(AddTechnicalServiceStaffRequest request)
        {
            var result = new ServiceResult();
            var exist = await _context.TechnicalServiceStaffs.FirstOrDefaultAsync(x => x.FullName == request.FullName && x.MobilePhone == request.MobilePhone);
            if (exist != null)
            {
                result.SetError(_localizer["RecordAlreadyExist"]);
                return result;
            }
            var technicServiceStaff = new TechnicalServiceStaff();
            technicServiceStaff.FullName = request.FullName;
            technicServiceStaff.MobilePhone = request.MobilePhone;
            technicServiceStaff.TechnicalServiceNameID = request.TechnicalServiceNameID;
            _context.TechnicalServiceStaffs.Add(technicServiceStaff);
            await _context.SaveChangesAsync();

            result.SetSuccess(_localizer["RecordAdded"]);
            return result;
        }

        public async Task<ServiceResult> DeleteTechnicalServiceStaff(DeleteTechnicalServiceStaffRequest request)
        {
            var result = new ServiceResult();
            var technicalServiceStaff = await _context.TechnicalServiceStaffs.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (technicalServiceStaff == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            technicalServiceStaff.IsEnabled = false;
            _context.TechnicalServiceStaffs.Update(technicalServiceStaff);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordDeleted"]);
            return result;
        }

        public async Task<ServiceResult<List<ListTechnicalServiceStaffsResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListTechnicalServiceStaffsResponse>>();
            if (_context.TechnicalServiceStaffs.Count() == 0)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            var technicalStaffs = await (from tech in _context.TechnicalServiceStaffs
                                         where tech.IsEnabled == true
                                         join ps in _context.TechnicalServiceNames on tech.TechnicalServiceNameID equals ps.ID
                                         select new ListTechnicalServiceStaffsResponse
                                         {
                                             ID = tech.ID,
                                             FullName = tech.FullName,
                                             MobilePhone = tech.MobilePhone,
                                             TechnicalServiceName = ps.Name
                                         }).ToListAsync();
            result.Data = technicalStaffs;
            return result;
        }

        public async Task<ServiceResult<GetTechnicalServiceStaffResponse>> GetTechnicalServiceStaff(int TechnicalServiceStaffID)
        {
            var result = new ServiceResult<GetTechnicalServiceStaffResponse>();
            var technicalStaff = await (from tech in _context.TechnicalServiceStaffs
                                        where tech.ID == TechnicalServiceStaffID && tech.IsEnabled == true
                                        join ps in _context.TechnicalServiceNames on tech.TechnicalServiceNameID equals ps.ID
                                        select new GetTechnicalServiceStaffResponse
                                        {
                                            ID = tech.ID,
                                            FullName = tech.FullName,
                                            MobilePhone = tech.MobilePhone,
                                            TechnicalServiceName = ps.Name,
                                        }).FirstAsync();
            if (technicalStaff == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = technicalStaff;
            return result;
        }

        public async Task<ServiceResult<UpdateTechnicalServiceStaffRequest>> UpdateTechnicalServiceStaff(int TechnicalServiceStaffID)
        {
            var result = new ServiceResult<UpdateTechnicalServiceStaffRequest>();
            var technicalStaff = await (from tech in _context.TechnicalServiceStaffs
                                        where tech.ID == TechnicalServiceStaffID && tech.IsEnabled == true
                                        join ps in _context.TechnicalServiceNames on tech.TechnicalServiceNameID equals ps.ID
                                        select new UpdateTechnicalServiceStaffRequest
                                        {
                                            ID = tech.ID,
                                            FullName = tech.FullName,
                                            MobilePhone = tech.MobilePhone,
                                            TechnicalServiceName = ps.Name,
                                        }).FirstAsync();
            if (technicalStaff == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            result.Data = technicalStaff;
            return result;
        }

        public async Task<ServiceResult> UpdateTechnicalServiceStaff(UpdateTechnicalServiceStaffRequest request)
        {
            var result = new ServiceResult();
            var technicalStaff = await _context.TechnicalServiceStaffs.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (technicalStaff == null)
            {
                result.SetError(_localizer["RecordNotFound"]);
                return result;
            }
            technicalStaff.FullName = request.FullName;
            technicalStaff.MobilePhone = request.MobilePhone;
            technicalStaff.TechnicalServiceNameID = request.TechnicalServiceNameID;
            _context.TechnicalServiceStaffs.Update(technicalStaff);
            await _context.SaveChangesAsync();
            result.SetSuccess(_localizer["RecordUpdated"]);
            return result;
        }
    }
}