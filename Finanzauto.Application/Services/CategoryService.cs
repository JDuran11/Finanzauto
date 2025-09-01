using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Category;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICloudinaryService _cloudinaryService;

        public CategoryService
        (
            ICategoryRepository categoryRepository, 
            ILogger<CategoryService> logger,
            ICloudinaryService cloudinary
        )
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _cloudinaryService = cloudinary;
        }

        public async Task<CategoryDTO> CreateCategory(CategoryCreateDTO dto, Stream? pictureStream, string? fileName)
        {
            try
            {
                if (pictureStream != null && !string.IsNullOrWhiteSpace(fileName))
                {
                    var pictureUrl = await _cloudinaryService.UploadImageAsync(pictureStream, fileName);
                    dto.Picture = pictureUrl;
                }


                return await _categoryRepository.CreateCategory(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando la categoría {@dto}", dto);
                throw;
            }
        }

        public async Task<CategoryDTO?> GetCategoryById(long id)
        {
            try
            {
                return await _categoryRepository.GetCategoryById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo categoría con id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesPaged(int page, int pageSize)
        {
            try
            {
                return await _categoryRepository.GetCategoriesPaged(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando categorías paginadas. Page {Page}, PageSize {PageSize}", page, pageSize);
                throw;
            }
        }

        public async Task<bool> UpdateCategory(long id, CategoryCreateDTO dto, Stream? pictureStream, string? fileName)
        {
            try
            {
                if (pictureStream != null && !string.IsNullOrWhiteSpace(fileName))
                {
                    var pictureUrl = await _cloudinaryService.UploadImageAsync(pictureStream, fileName);
                    dto.Picture = pictureUrl;
                }

                return await _categoryRepository.UpdateCategory(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando categoría {Id} con datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteCategory(long id)
        {
            try
            {
                return await _categoryRepository.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando categoría con id {Id}", id);
                throw;
            }
        }
    }
}
