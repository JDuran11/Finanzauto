using Finanzauto.Domain.DTOS.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDTO> CreateEmployee(EmployeeCreateDTO dto);
        Task<EmployeeDTO?> GetEmployeeById(long id);
        Task<IEnumerable<EmployeeDTO>> GetEmployeesPaged(int page, int pageSize);
        Task<bool> UpdateEmployee(long id, EmployeeCreateDTO dto);
        Task<bool> DeleteEmployee(long id);
    }
}
