using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO request)
        {
            try
            {
                var order = await _orderService.CreateOrder(request);
                if (order == null)
                    return BadRequest("No se pudo crear la orden.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando orden");
                return StatusCode(500, "Ocurrió un error al crear la orden.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                    return NotFound($"Orden con id {id} no encontrada.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo orden {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener la orden.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Orders")]
        public async Task<IActionResult> GetOrdersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetOrdersPaged(page, pageSize);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando órdenes paginadas. Page {Page}, PageSize {PageSize}", page, pageSize);
                return StatusCode(500, "Ocurrió un error al obtener las órdenes.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] OrderCreateDTO request)
        {
            try
            {
                var success = await _orderService.UpdateOrder(id, request);
                if (!success)
                    return NotFound($"Orden con id {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando orden {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar la orden.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            try
            {
                var success = await _orderService.DeleteOrder(id);
                if (!success)
                    return NotFound($"Orden con id {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando orden {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar la orden.");
            }
        }
    }
}
