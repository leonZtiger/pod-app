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
    public class PodModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public required string Duration { get; set; }
        public required string URL { get; set; }
        public required string Category { get; set; }
        public required bool IsSaved { get; set; }
    }
}
