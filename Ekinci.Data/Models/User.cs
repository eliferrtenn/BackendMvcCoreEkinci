﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Ekinci.Data.Models
{
    [Table("User.Users")]
    public class User
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public bool IsEnabled { get; set; }
        [NotMapped]
        public string FullName { get { return $"{Firstname} {Lastname}"; } }
    }
}