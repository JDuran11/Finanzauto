using Finanzauto.Domain.DTOS.Shipper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface IShipperService
    {
        Task<ShipperDTO> AddShipper(ShipperCreateDTO dto);
        Task<IEnumerable<ShipperDTO>> GetAllShippers();
        Task<ShipperDTO?> GetShipperById(long id);
        Task<ShipperDTO?> UpdateShipper(long id, ShipperCreateDTO dto);
        Task<bool> DeleteShipper(long id);
    }
}
