using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Employee;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finanzauto.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ICloudinaryService _cloudinaryService;

        public EmployeeService
        (
            IEmployeeRepository employeeRepository,
            ICloudinaryService cloudinary,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _cloudinaryService = cloudinary;
            _logger = logger;
        }

        public async Task<EmployeeDTO> CreateEmployee(EmployeeCreateDTO dto, Stream? pictureStream, string? fileName)
        {
            try
            {
                if (pictureStream != null && !string.IsNullOrWhiteSpace(fileName))
                {
                    var pictureUrl = await _cloudinaryService.UploadImageAsync(pictureStream, fileName);
                    dto.Photo = pictureUrl;
                }

                return await _employeeRepository.CreateEmployee(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando empleado {@dto}", dto);
                throw;
            }
        }

        public async Task<EmployeeDTO?> GetEmployeeById(long id)
        {
            try
            {
                return await _employeeRepository.GetEmployeeById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo empleado con id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesPaged(int page, int pageSize)
        {
            try
            {
                return await _employeeRepository.GetEmployeesPaged(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando empleados paginados. Page {Page}, PageSize {PageSize}", page, pageSize);
                throw;
            }
        }

        public async Task<bool> UpdateEmployee(long id, EmployeeCreateDTO dto, Stream? pictureStream, string? fileName)
        {
            try
            {
                if (pictureStream != null && !string.IsNullOrWhiteSpace(fileName))
                {
                    var pictureUrl = await _cloudinaryService.UploadImageAsync(pictureStream, fileName);
                    dto.Photo = pictureUrl;
                }

                return await _employeeRepository.UpdateEmployee(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando empleado {Id} con datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteEmployee(long id)
        {
            try
            {
                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando empleado con id {Id}", id);
                throw;
            }
        }
    }
}
