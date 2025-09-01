using Finanzauto.Domain.DTOS.Category;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finanzauto.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public CategoryRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<CategoryDTO> CreateCategory(CategoryCreateDTO dto)
        {
            var entity = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                Picture = dto.Picture
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return new CategoryDTO
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                Picture = entity.Picture,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<CategoryDTO?> GetCategoryById(long id)
        {
            string cacheKey = $"Category_{id}";
            if (!_cache.TryGetValue(cacheKey, out CategoryDTO? cachedCategory))
            {
                cachedCategory = await _context.Categories
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Description = c.Description,
                        Picture = c.Picture,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (cachedCategory != null)
                {
                    _cache.Set(cacheKey, cachedCategory, TimeSpan.FromMinutes(5));
                }
            }

            return cachedCategory;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesPaged(int page, int pageSize)
        {
            string cacheKey = $"Categories_Page_{page}_Size_{pageSize}";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<CategoryDTO>? cachedList))
            {
                cachedList = await _context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Description = c.Description,
                        Picture = c.Picture,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, cachedList, TimeSpan.FromMinutes(5));
            }

            return cachedList ?? Enumerable.Empty<CategoryDTO>();
        }

        public async Task<bool> UpdateCategory(long id, CategoryCreateDTO dto)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;

            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;
            entity.Picture = dto.Picture;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategory(long id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return false;

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
