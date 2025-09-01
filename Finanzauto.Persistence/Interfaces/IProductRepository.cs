using Finanzauto.Domain.DTOS.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductDTO> CreateProduct(ProductCreateDTO dto);
        Task<ProductDTO?> GetProductById(long id);
        Task<IEnumerable<ProductDTO>> GetProductsPaged(int page,int pageSize, string? productName = null, long? categoryId = null, long? supplierId = null, bool? discontinued = null);
        Task<bool> UpdateProduct(long id, ProductCreateDTO dto);
        Task<bool> DeleteProduct(long id);
        Task<int> CreateProductsBulk(IEnumerable<ProductCreateDTO> productsDto);
    }
}
