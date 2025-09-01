using Finanzauto.Application.Interfaces;
using Finanzauto.Application.Services;
using Finanzauto.Domain.DTOS.Category;
using Finanzauto.Domain.DTOS.Employee;
using Finanzauto.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeCreateRequest request)
        {
            try
            {
                var dto = new EmployeeCreateDTO
                {
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    Title = request.Title,
                    TitleOfCourtesy = request.TitleOfCourtesy,
                    BirthDate = request.BirthDate,
                    HireDate = request.HireDate,
                    Address = request.Address,
                    City = request.City,
                    Region = request.Region,
                    PostalCode = request.PostalCode,
                    Country = request.Country,
                    HomePhone = request.HomePhone,
                    Extension = request.Extension,
                    Notes = request.Notes,
                    ReportsTo = request.ReportsTo,
                };

                Stream? pictureStream = null;
                string? fileName = null;

                if (request.PhotoFile != null)
                {
                    pictureStream = request.PhotoFile.OpenReadStream();
                    fileName = request.PhotoFile.FileName;
                }

                var result = await _employeeService.CreateEmployee(dto, pictureStream, fileName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando empleado");
                return StatusCode(500, "Ocurrió un error al crear el empleado.");
            }
        }

        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateEmployee(long id, [FromForm] EmployeeCreateRequest request)
        {
            try
            {
                var dto = new EmployeeCreateDTO
                {
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    Title = request.Title,
                    TitleOfCourtesy = request.TitleOfCourtesy,
                    BirthDate = request.BirthDate,
                    HireDate = request.HireDate,
                    Address = request.Address,
                    City = request.City,
                    Region = request.Region,
                    PostalCode = request.PostalCode,
                    Country = request.Country,
                    HomePhone = request.HomePhone,
                    Extension = request.Extension,
                    Notes = request.Notes,
                    ReportsTo = request.ReportsTo,
                };

                Stream? pictureStream = null;
                string? fileName = null;

                if (request.PhotoFile != null)
                {
                    pictureStream = request.PhotoFile.OpenReadStream();
                    fileName = request.PhotoFile.FileName;
                }

                var success = await _employeeService.UpdateEmployee(id, dto, pictureStream, fileName);
                if (!success)
                    return NotFound($"Empleado con id {id} no encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando empleado {Id}", id);
                return StatusCode(500, "Ocurrió un error al actualizar el empleado.");
            }
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetEmployeeById(long id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeById(id);
                if (employee == null)
                    return NotFound($"Empleado con id {id} no encontrado.");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo empleado {Id}", id);
                return StatusCode(500, "Ocurrió un error al obtener el empleado.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/api/Employees")]
        public async Task<IActionResult> GetEmployeesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesPaged(page, pageSize);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando empleados paginados. Page {Page}, PageSize {PageSize}", page, pageSize);
                return StatusCode(500, "Ocurrió un error al obtener los empleados.");
            }
        }

        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            try
            {
                var success = await _employeeService.DeleteEmployee(id);
                if (!success)
                    return NotFound($"Empleado con id {id} no encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando empleado {Id}", id);
                return StatusCode(500, "Ocurrió un error al eliminar el empleado.");
            }
        }
    }
}
