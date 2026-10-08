using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSalesApi.Models
{
    public class SaleRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string SellerName { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}