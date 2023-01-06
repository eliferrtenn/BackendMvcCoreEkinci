using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Service.TechnicalServiceNames")]
    public class TechnicalServiceName
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}