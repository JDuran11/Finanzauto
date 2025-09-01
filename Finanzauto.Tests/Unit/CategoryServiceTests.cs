using Moq;
using Xunit;
using Finanzauto.Application.Services;
using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Category;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Finanzauto.Persistence.Interfaces;

namespace Finanzauto.Tests.Unit
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task CreateCategory_ShouldReturnCategoryDTO_WhenSuccessful()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var inputDto = new CategoryCreateDTO
            {
                CategoryName = "Electrónica",
                Description = "Productos electrónicos"
            };

            var expectedDto = new CategoryDTO
            {
                Id = 1,
                CategoryName = inputDto.CategoryName,
                Description = inputDto.Description
            };

            mockRepo.Setup(r => r.CreateCategory(It.IsAny<CategoryCreateDTO>()))
                    .ReturnsAsync(expectedDto);

            mockCloudinary.Setup(c => c.UploadImageAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                          .ReturnsAsync("http://fakeurl.com/image.jpg");

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            using var fakeStream = new MemoryStream();

            var result = await service.CreateCategory(inputDto, fakeStream, "image.jpg");

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.CategoryName, result.CategoryName);
            Assert.Equal(expectedDto.Description, result.Description);

            mockRepo.Verify(r => r.CreateCategory(It.IsAny<CategoryCreateDTO>()), Times.Once);
            mockCloudinary.Verify(c => c.UploadImageAsync(It.IsAny<Stream>(), "image.jpg"), Times.Once);
        }

        [Fact]
        public async Task CreateCategory_ShouldSkipCloudinary_WhenNoImageProvided()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var inputDto = new CategoryCreateDTO
            {
                CategoryName = "Hogar",
                Description = "Productos para el hogar"
            };

            var expectedDto = new CategoryDTO
            {
                Id = 2,
                CategoryName = inputDto.CategoryName,
                Description = inputDto.Description
            };

            mockRepo.Setup(r => r.CreateCategory(It.IsAny<CategoryCreateDTO>()))
                    .ReturnsAsync(expectedDto);

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            var result = await service.CreateCategory(inputDto, null, null);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Null(inputDto.Picture);

            mockCloudinary.Verify(c => c.UploadImageAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
            mockRepo.Verify(r => r.CreateCategory(It.IsAny<CategoryCreateDTO>()), Times.Once);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory_WhenExists()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var categoryId = 1L;
            var expectedCategory = new CategoryDTO
            {
                Id = categoryId,
                CategoryName = "Electrónica",
                Description = "Productos electrónicos"
            };

            mockRepo.Setup(r => r.GetCategoryById(categoryId))
                    .ReturnsAsync(expectedCategory);

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            var result = await service.GetCategoryById(categoryId);

            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Electrónica", result.CategoryName);

            mockRepo.Verify(r => r.GetCategoryById(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetCategoriesPaged_ShouldReturnPagedCategories()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var expectedCategories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, CategoryName = "Electrónica" },
                new CategoryDTO { Id = 2, CategoryName = "Hogar" }
            };

            mockRepo.Setup(r => r.GetCategoriesPaged(1, 10))
                    .ReturnsAsync(expectedCategories);

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            var result = await service.GetCategoriesPaged(1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.CategoryName == "Electrónica");
            Assert.Contains(result, c => c.CategoryName == "Hogar");

            mockRepo.Verify(r => r.GetCategoriesPaged(1, 10), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnTrue_WhenSuccessful()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var id = 1L;
            var inputDto = new CategoryCreateDTO
            {
                CategoryName = "Electrónica Actualizada",
                Description = "Actualización"
            };

            mockRepo.Setup(r => r.UpdateCategory(id, It.IsAny<CategoryCreateDTO>()))
                    .ReturnsAsync(true);

            mockCloudinary.Setup(c => c.UploadImageAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                          .ReturnsAsync("http://fakeurl.com/image-updated.jpg");

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            using var fakeStream = new MemoryStream();

            var result = await service.UpdateCategory(id, inputDto, fakeStream, "image.jpg");

            Assert.True(result);
            Assert.Equal("http://fakeurl.com/image-updated.jpg", inputDto.Picture);

            mockRepo.Verify(r => r.UpdateCategory(id, It.IsAny<CategoryCreateDTO>()), Times.Once);
            mockCloudinary.Verify(c => c.UploadImageAsync(fakeStream, "image.jpg"), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnTrue_WhenSuccessful()
        {
            var mockRepo = new Mock<ICategoryRepository>();
            var mockLogger = new Mock<ILogger<CategoryService>>();
            var mockCloudinary = new Mock<ICloudinaryService>();

            var id = 1L;

            mockRepo.Setup(r => r.DeleteCategory(id))
                    .ReturnsAsync(true);

            var service = new CategoryService(mockRepo.Object, mockLogger.Object, mockCloudinary.Object);

            var result = await service.DeleteCategory(id);

            Assert.True(result);
            mockRepo.Verify(r => r.DeleteCategory(id), Times.Once);
        }

    }
}
