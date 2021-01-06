using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Congo.Models
{
    public class Book
    {
        // designate this property as the document's primary key.
        [BsonId]
        // allow passing the parameter as type string instead of an ObjectId structure.
        // Mongo handles the conversion from string to ObjectId.
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        // Name represents the property name in the MongoDB collection.
        [BsonElement("Name")]
        // Serialised as Name in JSON, and not BookName (member casing) or bookName (default)
        [JsonProperty("Name")]
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}