using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.Models
{
    /// <summary>
    /// Model that represents a podcast episode. 
    /// 
    /// Contains the title, description, image url,
    /// release date, url for the actual podcast site,
    /// category and a per-user defined isSaved field.
    /// </summary>
    public class Episode 
    {
        public string PodcastFeedId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public DateTime ReleaseDate { get; set; } = default;
        public string Duration { get; set; } = default!;
        public string URL { get; set; } = default!;
        public string Category { get; set; } = default!;
        public int EpisodeNum { get; set; } = default;
        public bool IsExpanded { get; set; }

    }
}
