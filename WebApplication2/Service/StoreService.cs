using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoSalesApi.Models;

namespace MongoSalesApi.Services
{
    public class StoreService
    {
        private readonly IMongoCollection<PriceListItem> _priceListCollection;
        private readonly IMongoCollection<SaleRecord> _salesCollection;

        public StoreService(IOptions<MongoDbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _priceListCollection = mongoDatabase.GetCollection<PriceListItem>(dbSettings.Value.PriceListCollectionName);
            _salesCollection = mongoDatabase.GetCollection<SaleRecord>(dbSettings.Value.SalesCollectionName);
        }

        public async Task<List<SellerReportDto>> GetSellersReportAsync()
        {
            var report = await _salesCollection.Aggregate()
                .Group(s => s.SellerName, g => new SellerReportDto
                {
                    SellerName = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity),
                    TotalRevenue = g.Sum(s => s.TotalCost)
                })
                .SortByDescending(r => r.TotalRevenue)
                .ToListAsync();

            return report;
        }

        public async Task SeedDataAsync()
        {
            var pricesCount = await _priceListCollection.CountDocumentsAsync(_ => true);
            if (pricesCount == 0)
            {
                var products = new List<PriceListItem>
                {
                    new PriceListItem { ProductCode = "P01", ProductName = "Ноутбук", UnitPrice = 50000 },
                    new PriceListItem { ProductCode = "P02", ProductName = "Мышь", UnitPrice = 1000 },
                    new PriceListItem { ProductCode = "P03", ProductName = "Клавиатура", UnitPrice = 2000 },
                    new PriceListItem { ProductCode = "P04", ProductName = "Монитор", UnitPrice = 15000 },
                    new PriceListItem { ProductCode = "P05", ProductName = "Принтер", UnitPrice = 8000 },
                    new PriceListItem { ProductCode = "P06", ProductName = "Кабель HDMI", UnitPrice = 500 },
                    new PriceListItem { ProductCode = "P07", ProductName = "Флешка 64GB", UnitPrice = 800 }
                };
                await _priceListCollection.InsertManyAsync(products);

                var sellers = new[] { "Иванов И.И.", "Петров П.П.", "Сидоров С.С.", "Смирнова А.А.", "Кузнецов В.В.", "Попов Д.Д.", "Васильев Е.Е." };

                var sales = new List<SaleRecord>
                {
                    new SaleRecord { SellerName = sellers[0], ProductCode = "P01", ProductName = "Ноутбук", Quantity = 1, TotalCost = 50000 * 1 },
                    new SaleRecord { SellerName = sellers[1], ProductCode = "P02", ProductName = "Мышь", Quantity = 2, TotalCost = 1000 * 2 },
                    new SaleRecord { SellerName = sellers[2], ProductCode = "P03", ProductName = "Клавиатура", Quantity = 3, TotalCost = 2000 * 3 },
                    new SaleRecord { SellerName = sellers[3], ProductCode = "P04", ProductName = "Монитор", Quantity = 4, TotalCost = 15000 * 4 },
                    new SaleRecord { SellerName = sellers[4], ProductCode = "P05", ProductName = "Принтер", Quantity = 5, TotalCost = 8000 * 5 },
                    new SaleRecord { SellerName = sellers[5], ProductCode = "P06", ProductName = "Кабель HDMI", Quantity = 10, TotalCost = 500 * 10 },
                    new SaleRecord { SellerName = sellers[6], ProductCode = "P07", ProductName = "Флешка 64GB", Quantity = 15, TotalCost = 800 * 15 },

                    new SaleRecord { SellerName = sellers[0], ProductCode = "P02", ProductName = "Мышь", Quantity = 20, TotalCost = 1000 * 20 },
                    new SaleRecord { SellerName = sellers[1], ProductCode = "P03", ProductName = "Клавиатура", Quantity = 25, TotalCost = 2000 * 25 },
                    new SaleRecord { SellerName = sellers[2], ProductCode = "P06", ProductName = "Кабель HDMI", Quantity = 50, TotalCost = 500 * 50 },

                    new SaleRecord { SellerName = sellers[3], ProductCode = "P01", ProductName = "Ноутбук", Quantity = 2, TotalCost = 50000 * 2 },
                    new SaleRecord { SellerName = sellers[4], ProductCode = "P04", ProductName = "Монитор", Quantity = 3, TotalCost = 15000 * 3 },
                    new SaleRecord { SellerName = sellers[5], ProductCode = "P07", ProductName = "Флешка 64GB", Quantity = 5, TotalCost = 800 * 5 },
                    new SaleRecord { SellerName = sellers[6], ProductCode = "P02", ProductName = "Мышь", Quantity = 1, TotalCost = 1000 * 1 },
                    new SaleRecord { SellerName = sellers[0], ProductCode = "P05", ProductName = "Принтер", Quantity = 4, TotalCost = 8000 * 4 }
                };
                await _salesCollection.InsertManyAsync(sales);
            }
        }
    }
}