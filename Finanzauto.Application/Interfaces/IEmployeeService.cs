using Finanzauto.Domain.DTOS.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> CreateEmployee(EmployeeCreateDTO dto, Stream? pictureStream, string? fileName);
        Task<EmployeeDTO?> GetEmployeeById(long id);
        Task<IEnumerable<EmployeeDTO>> GetEmployeesPaged(int page, int pageSize);
        Task<bool> UpdateEmployee(long id, EmployeeCreateDTO dto, Stream? pictureStream, string? fileName);
        Task<bool> DeleteEmployee(long id);
    }
}
