using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Project.Projects")]
    public class Projects
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ThumbUrl { get; set; }
        public string FileUrl { get; set; }
        public DateTime ProjectDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ApartmentCount { get; set; }
        public int SquareMeter { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}