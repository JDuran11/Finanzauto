using Finanzauto.Application.Services;
using Finanzauto.Domain.DTOS.Product;
using Microsoft.Extensions.Logging;
using Moq;
using Finanzauto.Persistence.Interfaces;

namespace Finanzauto.Tests.Unit
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_ShouldReturnProductDTO_WhenSuccessful()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var inputDto = new ProductCreateDTO
            {
                ProductName = "Producto Test",
                SupplierId = 1,
                CategoryId = 2,
                QuantityPerUnit = "10 cajas",
                UnitPrice = 100,
                UnitsInStock = 50,
                UnitsOnOrder = 10,
                ReorderLevel = 5,
                Discontinued = false
            };

            var expectedDto = new ProductDTO
            {
                Id = 1,
                ProductName = inputDto.ProductName,
                SupplierId = inputDto.SupplierId,
                SupplierName = "Proveedor Test",
                CategoryId = inputDto.CategoryId,
                CategoryName = "Categoría Test",
                QuantityPerUnit = inputDto.QuantityPerUnit,
                UnitPrice = inputDto.UnitPrice,
                UnitsInStock = inputDto.UnitsInStock,
                UnitsOnOrder = inputDto.UnitsOnOrder,
                ReorderLevel = inputDto.ReorderLevel,
                Discontinued = inputDto.Discontinued,
                CategoryImageUrl = "http://fakeimage.com/image.jpg"
            };

            mockRepo.Setup(r => r.CreateProduct(It.IsAny<ProductCreateDTO>()))
                    .ReturnsAsync(expectedDto);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var result = await service.CreateProduct(inputDto);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.ProductName, result.ProductName);
            Assert.Equal(expectedDto.SupplierId, result.SupplierId);
            Assert.Equal(expectedDto.CategoryId, result.CategoryId);
            Assert.Equal(expectedDto.QuantityPerUnit, result.QuantityPerUnit);
            Assert.Equal(expectedDto.UnitPrice, result.UnitPrice);
            Assert.Equal(expectedDto.UnitsInStock, result.UnitsInStock);
            Assert.Equal(expectedDto.UnitsOnOrder, result.UnitsOnOrder);
            Assert.Equal(expectedDto.ReorderLevel, result.ReorderLevel);
            Assert.Equal(expectedDto.Discontinued, result.Discontinued);
            Assert.Equal(expectedDto.SupplierName, result.SupplierName);
            Assert.Equal(expectedDto.CategoryName, result.CategoryName);
            Assert.Equal(expectedDto.CategoryImageUrl, result.CategoryImageUrl);

            mockRepo.Verify(r => r.CreateProduct(It.IsAny<ProductCreateDTO>()), Times.Once);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenExists()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var productId = 1L;
            var expectedProduct = new ProductDTO
            {
                Id = productId,
                ProductName = "Producto 1",
                SupplierId = 1,
                SupplierName = "Proveedor 1",
                CategoryId = 2,
                CategoryName = "Categoría 1",
                UnitPrice = 50,
                UnitsInStock = 20,
                Discontinued = false
            };

            mockRepo.Setup(r => r.GetProductById(productId))
                    .ReturnsAsync(expectedProduct);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var result = await service.GetProductById(productId);

            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Producto 1", result.ProductName);

            mockRepo.Verify(r => r.GetProductById(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductsPaged_ShouldReturnPagedProducts()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var products = new List<ProductDTO>
            {
                new ProductDTO { Id = 1, ProductName = "Producto A" },
                new ProductDTO { Id = 2, ProductName = "Producto B" }
            };

            mockRepo.Setup(r => r.GetProductsPaged(1, 10, null, null, null, null))
                    .ReturnsAsync(products);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var result = await service.GetProductsPaged(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            mockRepo.Verify(r => r.GetProductsPaged(1, 10, null, null, null, null), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnTrue_WhenSuccessful()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var id = 1L;
            var dto = new ProductCreateDTO
            {
                ProductName = "Actualizado",
                SupplierId = 1,
                CategoryId = 2
            };

            mockRepo.Setup(r => r.UpdateProduct(id, It.IsAny<ProductCreateDTO>())).ReturnsAsync(true);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var result = await service.UpdateProduct(id, dto);

            Assert.True(result);
            mockRepo.Verify(r => r.UpdateProduct(id, It.IsAny<ProductCreateDTO>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_WhenSuccessful()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var id = 1L;
            mockRepo.Setup(r => r.DeleteProduct(id)).ReturnsAsync(true);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var result = await service.DeleteProduct(id);

            Assert.True(result);
            mockRepo.Verify(r => r.DeleteProduct(id), Times.Once);
        }

        [Fact]
        public async Task BulkCreateProducts_ShouldReturnInsertedCount()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            int count = 3;
            mockRepo.Setup(r => r.CreateProductsBulk(It.IsAny<IEnumerable<ProductCreateDTO>>()))
                    .ReturnsAsync(count);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var inserted = await service.BulkCreateProducts(count);

            Assert.Equal(count, inserted);
            mockRepo.Verify(r => r.CreateProductsBulk(It.IsAny<IEnumerable<ProductCreateDTO>>()), Times.Once);
        }

        [Fact]
        public async Task BulkInsertProductsAsync_ShouldReturnInsertedCount()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockLogger = new Mock<ILogger<ProductService>>();

            var products = new List<ProductCreateDTO>
            {
                new ProductCreateDTO { ProductName = "P1", SupplierId = 1, CategoryId = 1 },
                new ProductCreateDTO { ProductName = "P2", SupplierId = 1, CategoryId = 1 }
            };

            mockRepo.Setup(r => r.CreateProductsBulk(products)).ReturnsAsync(products.Count);

            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            var inserted = await service.BulkInsertProductsAsync(products);

            Assert.Equal(2, inserted);
            mockRepo.Verify(r => r.CreateProductsBulk(products), Times.Once);
        }
    }
}
