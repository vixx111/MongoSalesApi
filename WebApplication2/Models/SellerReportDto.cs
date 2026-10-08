namespace MongoSalesApi.Models
{
    public class SellerReportDto
    {
        public string SellerName { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}