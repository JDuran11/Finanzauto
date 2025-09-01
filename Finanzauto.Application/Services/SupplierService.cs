using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Supplier;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finanzauto.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService
        (
            ISupplierRepository supplierRepository, 
            ILogger<SupplierService> logger
        )
        {
            _supplierRepository = supplierRepository;
            _logger = logger;
        }

        public async Task<SupplierDTO> CreateSupplier(SupplierCreateDTO dto)
        {
            try
            {
                return await _supplierRepository.CreateSupplier(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando proveedor {@dto}", dto);
                throw;
            }
        }

        public async Task<SupplierDTO?> GetSupplierById(long id)
        {
            try
            {
                return await _supplierRepository.GetSupplierById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proveedor con id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SupplierDTO>> GetSuppliersPaged(int page, int pageSize)
        {
            try
            {
                return await _supplierRepository.GetSuppliersPaged(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando proveedores paginados. Page {Page}, PageSize {PageSize}", page, pageSize);
                throw;
            }
        }

        public async Task<bool> UpdateSupplier(long id, SupplierCreateDTO dto)
        {
            try
            {
                return await _supplierRepository.UpdateSupplier(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando proveedor {Id} con datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteSupplier(long id)
        {
            try
            {
                return await _supplierRepository.DeleteSupplier(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando proveedor con id {Id}", id);
                throw;
            }
        }
    }
}
