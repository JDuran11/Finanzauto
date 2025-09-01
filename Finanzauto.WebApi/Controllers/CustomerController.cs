using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO request)
        {
            try
            {
                var result = await _customerService.CreateCustomer(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cliente");
                return StatusCode(500, "Ocurrió un error al crear el cliente.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCustomerById(long id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null)
                    return NotFound($"Cliente con id {id} no encontrado.");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener el cliente.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Customers")]
        public async Task<IActionResult> GetCustomersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var customers = await _customerService.GetCustomerPaged(page, pageSize);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando clientes paginados. Page {Page}, PageSize {PageSize}", page, pageSize);
                return StatusCode(500, "Ocurrió un error al obtener los clientes.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCustomer(long id, [FromBody] CustomerCreateDTO request)
        {
            try
            {
                var success = await _customerService.UpdateCustomer(id, request);
                if (!success)
                    return NotFound($"Cliente con id {id} no encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando cliente {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar el cliente.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            try
            {
                var success = await _customerService.DeleteCustomer(id);
                if (!success)
                    return NotFound($"Cliente con id {id} no encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando cliente {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar el cliente.");
            }
        }
    }
}
