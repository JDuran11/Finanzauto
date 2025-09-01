using Finanzauto.Domain.DTOS.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategory(CategoryCreateDTO dto, Stream? pictureStream, string? fileName);
        Task<CategoryDTO?> GetCategoryById(long id);
        Task<IEnumerable<CategoryDTO>> GetCategoriesPaged(int page, int pageSize);
        Task<bool> UpdateCategory(long id, CategoryCreateDTO dto, Stream? pictureStream, string? fileName);
        Task<bool> DeleteCategory(long id);
    }
}
