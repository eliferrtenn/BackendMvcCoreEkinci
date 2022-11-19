using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Blog.Blogs")]
    public class Blog 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? BlogDate { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}