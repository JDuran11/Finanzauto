using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Shipper;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finanzauto.Application.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly ILogger<ShipperService> _logger;

        public ShipperService(IShipperRepository shipperRepository, ILogger<ShipperService> logger)
        {
            _shipperRepository = shipperRepository;
            _logger = logger;
        }

        public async Task<ShipperDTO> AddShipper(ShipperCreateDTO dto)
        {
            try
            {
                return await _shipperRepository.AddShipper(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando trasportista {@dto}", dto);
                throw;
            }
        }

        public async Task<IEnumerable<ShipperDTO>> GetAllShippers()
        {
            try
            {
                return await _shipperRepository.GetAllShippers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de trasportistas");
                throw;
            }
        }

        public async Task<ShipperDTO?> GetShipperById(long id)
        {
            try
            {
                return await _shipperRepository.GetShipperById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo trasportista con Id {Id}", id);
                throw;
            }
        }

        public async Task<ShipperDTO?> UpdateShipper(long id, ShipperCreateDTO dto)
        {
            try
            {
                return await _shipperRepository.UpdateShipper(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando trasportista {Id} con datos {@dto}", id, dto);
                throw;
            }
        }

        public async Task<bool> DeleteShipper(long id)
        {
            try
            {
                return await _shipperRepository.DeleteShipper(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando trasportista con Id {Id}", id);
                throw;
            }
        }
    }
}
