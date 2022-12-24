using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("Corporate.Intros")]
    public class Intro
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public int SquareMeter { get; set; }
        public int YearCount { get; set; }
        public int CommercialAreaCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}