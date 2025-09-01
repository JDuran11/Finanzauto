using Finanzauto.Domain.DTOS.Product;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql.Bulk;

namespace Finanzauto.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ProductRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ProductDTO> CreateProduct(ProductCreateDTO dto)
        {
            var entity = new Product
            {
                ProductName = dto.ProductName,
                SupplierId = dto.SupplierId,
                CategoryId = dto.CategoryId,
                QuantityPerUnit = dto.QuantityPerUnit,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock,
                UnitsOnOrder = dto.UnitsOnOrder,
                ReorderLevel = dto.ReorderLevel,
                Discontinued = dto.Discontinued
            };

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            return await MapToDTO(entity.Id);
        }

        public async Task<ProductDTO?> GetProductById(long id)
        {
            string cacheKey = $"Product_{id}";

            if (!_cache.TryGetValue(cacheKey, out ProductDTO? productDto))
            {
                productDto = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        SupplierId = p.SupplierId,
                        SupplierName = p.Supplier.CompanyName,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.CategoryName,
                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        ReorderLevel = p.ReorderLevel,
                        Discontinued = p.Discontinued,
                        CategoryImageUrl = p.Category.Picture,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (productDto != null)
                {
                    _cache.Set(cacheKey, productDto, TimeSpan.FromMinutes(5));
                }
            }

            return productDto;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsPaged(
            int page,
            int pageSize,
            string? productName = null,
            long? categoryId = null,
            long? supplierId = null,
            bool? discontinued = null)
        {
            string cacheKey = $"Products_Page_{page}_Size_{pageSize}_Name_{productName}_Cat_{categoryId}_Sup_{supplierId}_Disc_{discontinued}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ProductDTO>? products))
            {
                IQueryable<Product> query = _context.Products
                    .AsNoTracking()
                    .Include(p => p.Supplier)
                    .Include(p => p.Category);

                if (!string.IsNullOrWhiteSpace(productName))
                {
                    query = query.Where(p => p.ProductName.Contains(productName));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                if (supplierId.HasValue)
                {
                    query = query.Where(p => p.SupplierId == supplierId.Value);
                }

                if (discontinued.HasValue)
                {
                    query = query.Where(p => p.Discontinued == discontinued.Value);
                }

                products = await query
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        SupplierId = p.SupplierId,
                        SupplierName = p.Supplier.CompanyName,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.CategoryName,
                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        ReorderLevel = p.ReorderLevel,
                        Discontinued = p.Discontinued,
                        CategoryImageUrl = p.Category.Picture,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .ToListAsync();

                if (products.Any())
                {
                    _cache.Set(cacheKey, products, TimeSpan.FromMinutes(1));
                }
            }

            return products ?? Enumerable.Empty<ProductDTO>();
        }


        public async Task<bool> UpdateProduct(long id, ProductCreateDTO dto)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return false;

            entity.ProductName = dto.ProductName;
            entity.SupplierId = dto.SupplierId;
            entity.CategoryId = dto.CategoryId;
            entity.QuantityPerUnit = dto.QuantityPerUnit;
            entity.UnitPrice = dto.UnitPrice;
            entity.UnitsInStock = dto.UnitsInStock;
            entity.UnitsOnOrder = dto.UnitsOnOrder;
            entity.ReorderLevel = dto.ReorderLevel;
            entity.Discontinued = dto.Discontinued;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return false;

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateProductsBulk(IEnumerable<ProductCreateDTO> productsDto)
        {
            var products = productsDto.Select(dto => new Product
            {
                ProductName = dto.ProductName,
                SupplierId = dto.SupplierId,
                CategoryId = dto.CategoryId,
                QuantityPerUnit = dto.QuantityPerUnit,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock,
                UnitsOnOrder = dto.UnitsOnOrder,
                ReorderLevel = dto.ReorderLevel,
                Discontinued = dto.Discontinued
            }).ToList();

            var bulkUploader = new NpgsqlBulkUploader(_context);

            await bulkUploader.InsertAsync(products);

            return products.Count;
        }

        private async Task<ProductDTO> MapToDTO(long productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.CompanyName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    QuantityPerUnit = p.QuantityPerUnit,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    UnitsOnOrder = p.UnitsOnOrder,
                    ReorderLevel = p.ReorderLevel,
                    Discontinued = p.Discontinued,
                    CategoryImageUrl = p.Category.Picture,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstAsync();
        }
    }
}
