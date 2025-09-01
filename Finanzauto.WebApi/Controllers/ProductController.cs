using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO request)
        {
            try
            {
                var product = await _productService.CreateProduct(request);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto");
                return StatusCode(500, "Ocurrió un error al crear el producto.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo producto {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener el producto.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Products")]
        public async Task<IActionResult> GetProductsPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? productName = null,
            [FromQuery] long? categoryId = null,
            [FromQuery] long? supplierId = null,
            [FromQuery] bool? discontinued = null)
        {
            try
            {
                var products = await _productService.GetProductsPaged(
                    page,
                    pageSize,
                    productName,
                    categoryId,
                    supplierId,
                    discontinued
                );

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error obteniendo productos paginados. Page: {Page}, PageSize: {PageSize}, Filters: {@Filters}",
                    page, pageSize, new { productName, categoryId, supplierId, discontinued });

                return StatusCode(500, "Ocurrió un error al obtener los productos.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductCreateDTO request)
        {
            try
            {
                var success = await _productService.UpdateProduct(id, request);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar el producto.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                var success = await _productService.DeleteProduct(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando producto {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar el producto.");
            }
        }

        [Authorize]
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateProducts([FromQuery] int count = 1000, [FromQuery] long? categoryId = null, long? supplierId = null)
        {
            var inserted = await _productService.BulkCreateProducts(count, categoryId, supplierId);
            return Ok($"{inserted} productos generados y guardados exitosamente.");
        }

        [Authorize]
        [HttpPost("Masive")]
        public async Task<IActionResult> BulkInsertProducts([FromBody] IEnumerable<ProductCreateDTO> productsDto)
        {
            if (productsDto == null || !productsDto.Any())
                return BadRequest("Debe enviar al menos un producto.");

            try
            {
                var insertedCount = await _productService.BulkInsertProductsAsync(productsDto);
                return Ok(new
                {
                    Message = $"Se insertaron {insertedCount} productos exitosamente.",
                    Inserted = insertedCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la carga masiva de productos");
                return StatusCode(500, "Ocurrió un error al procesar la carga masiva.");
            }
        }
    }
}
