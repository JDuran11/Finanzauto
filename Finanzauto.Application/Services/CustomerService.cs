using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Customer;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finanzauto.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<CustomerDTO> CreateCustomer(CustomerCreateDTO dto)
        {
            try
            {
                return await _customerRepository.CreateCustomer(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cliente {@dto}", dto);
                throw;
            }
        }

        public async Task<CustomerDTO?> GetCustomerById(long id)
        {
            try
            {
                return await _customerRepository.GetCustomerById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente con id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomerPaged(int page, int pageSize)
        {
            try
            {
                return await _customerRepository.GetCustomerPaged(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando clientes paginados. Page {Page}, PageSize {PageSize}", page, pageSize);
                throw;
            }
        }

        public async Task<bool> UpdateCustomer(long id, CustomerCreateDTO dto)
        {
            try
            {
                return await _customerRepository.UpdateCustomer(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando cliente {Id} con datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteCustomer(long id)
        {
            try
            {
                return await _customerRepository.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando cliente con id {Id}", id);
                throw;
            }
        }
    }
}
