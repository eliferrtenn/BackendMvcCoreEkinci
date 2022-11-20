﻿namespace Ekinci.WebAPI.Business.Models.Responses.VideosResponse
{
    public class GetVideoResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}