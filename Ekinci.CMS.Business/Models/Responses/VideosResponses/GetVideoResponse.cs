﻿namespace Ekinci.CMS.Business.Models.Responses.VideosResponses
{
    public class GetVideoResponse
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
    }
}