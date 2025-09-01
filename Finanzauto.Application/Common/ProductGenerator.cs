using Bogus;
using Finanzauto.Domain.DTOS.Product;

namespace Finanzauto.Application.Common
{
    public static class ProductGenerator
    {
        public static List<ProductCreateDTO> GenerateFakeProducts(int count, long? categoryId = null, long? supplierId = null)
        {
            var faker = new Faker<ProductCreateDTO>("es")
                .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                .RuleFor(p => p.CategoryId, f => categoryId ?? f.Random.Int(1, 2))
                .RuleFor(p => p.SupplierId, f => supplierId ?? f.Random.Int(1, 2))
                .RuleFor(p => p.QuantityPerUnit, f => $"{f.Random.Int(1, 50)} unidades")
                .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(100, 10000)))
                .RuleFor(p => p.UnitsInStock, f => f.Random.Int(0, 500))
                .RuleFor(p => p.UnitsOnOrder, f => f.Random.Int(0, 200))
                .RuleFor(p => p.ReorderLevel, f => f.Random.Int(1, 50))
                .RuleFor(p => p.Discontinued, f => f.Random.Bool());

            return faker.Generate(count);
        }
    }
}
