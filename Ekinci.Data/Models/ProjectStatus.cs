using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Project.ProjectStatuses")]
    public class ProjectStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }
}