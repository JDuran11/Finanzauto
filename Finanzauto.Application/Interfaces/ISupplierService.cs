using Finanzauto.Domain.DTOS.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDTO> CreateSupplier(SupplierCreateDTO dto);
        Task<SupplierDTO?> GetSupplierById(long id);
        Task<IEnumerable<SupplierDTO>> GetSuppliersPaged(int page, int pageSize);
        Task<bool> UpdateSupplier(long id, SupplierCreateDTO dto);
        Task<bool> DeleteSupplier(long id);
    }
}
