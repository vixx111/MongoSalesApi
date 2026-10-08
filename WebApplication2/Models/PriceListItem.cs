using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSalesApi.Models
{
    public class PriceListItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
    }
}