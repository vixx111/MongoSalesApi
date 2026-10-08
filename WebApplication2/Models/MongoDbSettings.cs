namespace MongoSalesApi.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PriceListCollectionName { get; set; } = null!;
        public string SalesCollectionName { get; set; } = null!;
    }
}