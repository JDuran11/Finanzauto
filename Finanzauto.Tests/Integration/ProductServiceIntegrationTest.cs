using Finanzauto.Application.Services;
using Finanzauto.Domain.DTOS.Product;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Finanzauto.Tests.Integration
{
    public class ProductServiceIntegrationTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private ProductService GetProductService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repo = new ProductRepository(context, memoryCache);
            var logger = new Mock<ILogger<ProductService>>().Object;
            return new ProductService(repo, logger);
        }

        private async Task SeedRelationsAsync(AppDbContext context)
        {
            if (!context.Suppliers.Any())
            {
                context.Suppliers.Add(new Domain.Entities.Supplier
                {
                    Id = 1,
                    CompanyName = "Proveedor Test",
                    ContactName = "Contacto Test",
                    ContactTitle = "Título",
                    Address = "Dirección",
                    City = "Ciudad",
                    Region = "Región",
                    PostalCode = "0000",
                    Country = "País",
                    Phone = "000-0000"
                });
            }

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Domain.Entities.Category
                {
                    Id = 1,
                    CategoryName = "Categoría Test",
                    Description = "Descripción de prueba",
                    Picture = "imagen-test.jpg"
                });
            }

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateProduct_ShouldInsertProduct()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            var dto = new ProductCreateDTO
            {
                ProductName = "Producto Test",
                SupplierId = 1,
                CategoryId = 1,
                UnitPrice = 100,
                UnitsInStock = 50,
                Discontinued = false
            };

            var result = await service.CreateProduct(dto);

            Assert.NotNull(result);
            Assert.Equal("Producto Test", result.ProductName);

            var dbProduct = await context.Products.FindAsync(result.Id);
            Assert.NotNull(dbProduct);
            Assert.Equal("Producto Test", dbProduct.ProductName);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            var product = new Domain.Entities.Product
            {
                ProductName = "Producto Consulta",
                SupplierId = 1,
                CategoryId = 1,
                UnitPrice = 50,
                UnitsInStock = 20,
                Discontinued = false
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await service.GetProductById(product.Id);

            Assert.NotNull(result);
            Assert.Equal(product.ProductName, result.ProductName);
        }

        [Fact]
        public async Task GetProductsPaged_ShouldReturnPagedList()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            context.Products.AddRange(new List<Domain.Entities.Product>
            {
                new Domain.Entities.Product { ProductName = "A", SupplierId=1, CategoryId=1 },
                new Domain.Entities.Product { ProductName = "B", SupplierId=1, CategoryId=1 },
                new Domain.Entities.Product { ProductName = "C", SupplierId=1, CategoryId=1 }
            });
            await context.SaveChangesAsync();

            var result = await service.GetProductsPaged(1, 2);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnTrue()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            var product = new Domain.Entities.Product { ProductName = "Original", SupplierId = 1, CategoryId = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var updateDto = new ProductCreateDTO { ProductName = "Actualizado", SupplierId = 1, CategoryId = 1 };
            var result = await service.UpdateProduct(product.Id, updateDto);

            Assert.True(result);

            var updated = await context.Products.FindAsync(product.Id);
            Assert.Equal("Actualizado", updated.ProductName);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            var product = new Domain.Entities.Product { ProductName = "Eliminar", SupplierId = 1, CategoryId = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var result = await service.DeleteProduct(product.Id);

            Assert.True(result);
            var deleted = await context.Products.FindAsync(product.Id);
            Assert.Null(deleted);
        }

        [Fact(Skip = "Requires real PostgreSQL database for bulk insert")]
        public async Task BulkCreateProducts_ShouldReturnInsertedCount()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            int count = 5;
            var inserted = await service.BulkCreateProducts(count);

            Assert.Equal(count, inserted);
            Assert.Equal(count, context.Products.Count());
        }

        [Fact(Skip = "Requires real PostgreSQL database for bulk insert")]
        public async Task BulkInsertProductsAsync_ShouldReturnInsertedCount()
        {
            using var context = GetInMemoryDbContext();
            await SeedRelationsAsync(context);
            var service = GetProductService(context);

            var products = new List<ProductCreateDTO>
            {
                new ProductCreateDTO { ProductName = "P1", SupplierId=1, CategoryId=1 },
                new ProductCreateDTO { ProductName = "P2", SupplierId=1, CategoryId=1 }
            };

            var inserted = await service.BulkInsertProductsAsync(products);

            Assert.Equal(2, inserted);
            Assert.Equal(2, context.Products.Count());
        }
    }
}
