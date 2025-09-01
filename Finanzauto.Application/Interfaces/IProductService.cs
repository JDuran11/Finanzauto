using Finanzauto.Domain.DTOS.Product;

namespace Finanzauto.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> CreateProduct(ProductCreateDTO dto);
        Task<ProductDTO?> GetProductById(long id);
        Task<IEnumerable<ProductDTO>> GetProductsPaged(int page, int pageSize, string? productName = null, long? categoryId = null, long? supplierId = null, bool? discontinued = null);
        Task<bool> UpdateProduct(long id, ProductCreateDTO dto);
        Task<bool> DeleteProduct(long id);
        Task<int> BulkCreateProducts(int count, long? categoryId = null, long? supplierId = null);
        Task<int> BulkInsertProductsAsync(IEnumerable<ProductCreateDTO> productsDto);
    }
}
