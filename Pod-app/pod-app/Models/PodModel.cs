using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.Models
{
    internal class PodModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Duration { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public bool isSaved { get; set; }
    }
}
