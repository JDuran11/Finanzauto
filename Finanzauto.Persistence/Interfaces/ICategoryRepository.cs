using Finanzauto.Domain.DTOS.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Interfaces
{
    public interface ICategoryRepository
    {
        Task<CategoryDTO> CreateCategory(CategoryCreateDTO dto);
        Task<CategoryDTO?> GetCategoryById(long id);
        Task<IEnumerable<CategoryDTO>> GetCategoriesPaged(int page, int pageSize);
        Task<bool> UpdateCategory(long id, CategoryCreateDTO dto);
        Task<bool> DeleteCategory(long id);
    }
}
