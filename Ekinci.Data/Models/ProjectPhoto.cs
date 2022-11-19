using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Project.Project_Photos")]
    public class ProjectPhoto
    {
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;       
    }
}