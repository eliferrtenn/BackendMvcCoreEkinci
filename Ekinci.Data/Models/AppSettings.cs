using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ekinci.Data.Models
{
    [Table("Common.AppSettings")]
    public class AppSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Placeholder { get; set; }
        public int Order { get; set; }
    }
}