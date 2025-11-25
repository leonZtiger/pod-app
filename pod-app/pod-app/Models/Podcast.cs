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
    public class Podcast
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }



        [BsonIgnore]
        public List<Episode> Episodes { get; set; } // Filled by separated logic.

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonIgnore]
        public string ImageUrl { get; set; }


}
