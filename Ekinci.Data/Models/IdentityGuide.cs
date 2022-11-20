using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Company.IdentityGuides")]
    public class IdentityGuide
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}