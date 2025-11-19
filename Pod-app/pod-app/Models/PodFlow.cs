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
        public string Id { get; set; }   

        [BsonIgnore]
        public List<PodModel> Podcasts { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }
    
    }
}
