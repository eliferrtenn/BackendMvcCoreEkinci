using Ekinci.CMS.Business.Models.Responses.TechnicalServiceNameResponses;
using Ekinci.Common.Business;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface ITechnicalServiceNameService
    {
        Task<ServiceResult<List<ListTechnicalServiceNameResponse>>> GetAll();
    }
}