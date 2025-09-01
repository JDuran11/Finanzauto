using Finanzauto.Domain.DTOS.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDTO> CreateCustomer(CustomerCreateDTO dto);
        Task<CustomerDTO?> GetCustomerById(long id);
        Task<IEnumerable<CustomerDTO>> GetCustomerPaged(int page, int pageSize);
        Task<bool> UpdateCustomer(long id, CustomerCreateDTO dto);
        Task<bool> DeleteCustomer(long id);
    }
}
