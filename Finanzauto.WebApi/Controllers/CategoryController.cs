using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Category;
using Finanzauto.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            try
            {
                var dto = new CategoryCreateDTO
                {
                    CategoryName = request.CategoryName,
                    Description = request.Description
                };

                Stream? pictureStream = null;
                string? fileName = null;

                if (request.PictureFile != null)
                {
                    pictureStream = request.PictureFile.OpenReadStream();
                    fileName = request.PictureFile.FileName;
                }

                var result = await _categoryService.CreateCategory(dto, pictureStream, fileName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando categoría");
                return StatusCode(500, "Ocurrió un error al crear la categoría.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCategory(long id, [FromForm] CategoryCreateRequest request)
        {
            try
            {
                var dto = new CategoryCreateDTO
                {
                    CategoryName = request.CategoryName,
                    Description = request.Description
                };

                Stream? pictureStream = null;
                string? fileName = null;

                if (request.PictureFile != null)
                {
                    pictureStream = request.PictureFile.OpenReadStream();
                    fileName = request.PictureFile.FileName;
                }

                var success = await _categoryService.UpdateCategory(id, dto, pictureStream, fileName);
                if (!success)
                    return NotFound($"Categoría con id {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando categoría {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar la categoría.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCategoryById(long id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                    return NotFound($"Categoría con id {id} no encontrada.");

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo categoría {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener la categoría.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Categories")]
        public async Task<IActionResult> GetCategoriesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesPaged(page, pageSize);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando categorías paginadas. Page {Page}, PageSize {PageSize}", page, pageSize);
                return StatusCode(500, "Ocurrió un error al obtener las categorías.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            try
            {
                var success = await _categoryService.DeleteCategory(id);
                if (!success)
                    return NotFound($"Categoría con id {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando categoría {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar la categoría.");
            }
        }
    }
}
