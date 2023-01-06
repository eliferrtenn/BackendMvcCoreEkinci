using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Service.TechnicalServiceStaffs")]
    public class TechnicalServiceStaff
    {
        public int ID { get; set; }
        public int TechnicalServiceNameID { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}