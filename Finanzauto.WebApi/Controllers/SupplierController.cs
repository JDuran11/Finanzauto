using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierCreateDTO request)
        {
            try
            {
                var supplier = await _supplierService.CreateSupplier(request);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando proveedor");
                return StatusCode(500, "Ocurrió un error al crear el proveedor.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetSupplierById(long id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierById(id);
                if (supplier == null)
                    return NotFound();

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proveedor {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener el proveedor.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Suppliers")]
        public async Task<IActionResult> GetSuppliersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var suppliers = await _supplierService.GetSuppliersPaged(page, pageSize);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando proveedores paginados");
                return StatusCode(500, "Ocurrió un error al listar los proveedores.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateSupplier(long id, [FromBody] SupplierCreateDTO request)
        {
            try
            {
                var updated = await _supplierService.UpdateSupplier(id, request);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando proveedor {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar el proveedor.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            try
            {
                var deleted = await _supplierService.DeleteSupplier(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando proveedor {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar el proveedor.");
            }
        }
    }
}
