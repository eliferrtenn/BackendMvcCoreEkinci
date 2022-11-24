﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekinci.WebAPI.Business.Models.Responses.ContactResponse
{
    public class ListContactResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MobilePhone { get; set; }
        public string LandPhone { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}