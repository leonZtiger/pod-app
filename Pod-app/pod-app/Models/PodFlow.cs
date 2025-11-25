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
    /// 
    /// </summary>
    public class PodFlow
    {    
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }



        [BsonIgnore]
        public List<PodModel> Podcasts { get; set; }



        public bool IsExpanded { get; set; } = false;


    }
}
