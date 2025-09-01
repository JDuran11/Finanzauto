using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Shipper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        private readonly ILogger<ShipperController> _logger;

        public ShipperController(IShipperService shipperService, ILogger<ShipperController> logger)
        {
            _shipperService = shipperService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddShipper([FromBody] ShipperCreateDTO request)
        {
            try
            {
                var shipper = await _shipperService.AddShipper(request);
                return Ok(shipper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando transportista");
                return StatusCode(500, "Ocurrió un error al crear el transportista.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Shippers")]
        public async Task<IActionResult> GetAllShippers()
        {
            try
            {
                var shippers = await _shipperService.GetAllShippers();
                return Ok(shippers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo transportistas");
                return StatusCode(500, "Ocurrió un error al obtener los transportistas.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetShipperById(long id)
        {
            try
            {
                var shipper = await _shipperService.GetShipperById(id);
                if (shipper == null)
                    return NotFound();

                return Ok(shipper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo transportista {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener el transportista.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateShipper(long id, [FromBody] ShipperCreateDTO request)
        {
            try
            {
                var updated = await _shipperService.UpdateShipper(id, request);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando transportista {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar el transportista.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteShipper(long id)
        {
            try
            {
                var deleted = await _shipperService.DeleteShipper(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando transportista {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar el transportista.");
            }
        }
    }
}
