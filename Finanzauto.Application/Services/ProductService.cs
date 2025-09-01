using Finanzauto.Application.Common;
using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Product;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finanzauto.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ProductDTO> CreateProduct(ProductCreateDTO dto)
        {
            try
            {
                return await _productRepository.CreateProduct(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto {@dto}", dto);
                throw;
            }
        }

        public async Task<ProductDTO?> GetProductById(long id)
        {
            try
            {
                return await _productRepository.GetProductById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo producto con Id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsPaged(
            int page,
            int pageSize,
            string? productName = null,
            long? categoryId = null,
            long? supplierId = null,
            bool? discontinued = null)
        {
            try
            {
                return await _productRepository.GetProductsPaged(
                    page,
                    pageSize,
                    productName,
                    categoryId,
                    supplierId,
                    discontinued
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error obteniendo productos paginados. Page: {Page}, PageSize: {PageSize}, Filters: {@Filters}",
                    page,
                    pageSize,
                    new { productName, categoryId, supplierId, discontinued }
                );
                throw;
            }
        }


        public async Task<bool> UpdateProduct(long id, ProductCreateDTO dto)
        {
            try
            {
                return await _productRepository.UpdateProduct(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto con Id {Id} y datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteProduct(long id)
        {
            try
            {
                return await _productRepository.DeleteProduct(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando producto con Id {Id}", id);
                throw;
            }
        }

        public async Task<int> BulkCreateProducts(int count, long? categoryId = null, long? supplierId = null)
        {
            try
            {
                var fakeProducts = ProductGenerator.GenerateFakeProducts(count, categoryId, supplierId);

                var inserted = await _productRepository.CreateProductsBulk(fakeProducts);

                return inserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear productos masivamente. Count: {Count}, CategoryId: {CategoryId}, SupplierId: {SupplierId}",
                    count, categoryId, supplierId);
                throw;
            }
        }

        public async Task<int> BulkInsertProductsAsync(IEnumerable<ProductCreateDTO> productsDto)
        {
            try
            {
                if (productsDto == null || !productsDto.Any())
                    throw new ArgumentException("La lista de productos está vacía.");

                var inserted = await _productRepository.CreateProductsBulk(productsDto);
                return inserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la carga masiva de productos");
                throw;
            }
        }

    }
}
