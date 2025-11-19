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
    public class PodModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string PodcastFeedId { get; set; } = default!;

        [BsonElement("title")]
        public string Title { get; set; } = default!;

        [BsonElement("description")]
        public string Description { get; set; } = default!;

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = default!;

        [BsonElement("releaseDate")]
        [BsonDateTimeOptions(Representation = BsonType.Document)]
        public DateTime ReleaseDate { get; set; } = default;

        [BsonElement("duration")]
        public string Duration { get; set; } = default!;

        [BsonElement("url")]
        public string URL { get; set; } = default!;

        [BsonElement("category")]
        public string Category { get; set; } = default!;

        [BsonElement("isSaved")]
        public bool IsSaved { get; set; } = default!;
    }
}
