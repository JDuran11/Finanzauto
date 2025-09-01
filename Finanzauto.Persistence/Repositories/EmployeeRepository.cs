using Finanzauto.Domain.DTOS.Employee;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finanzauto.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public EmployeeRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;

        }

        public async Task<EmployeeDTO> CreateEmployee(EmployeeCreateDTO dto)
        {
            var entity = new Employee
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Title = dto.Title,
                TitleOfCourtesy = dto.TitleOfCourtesy,
                BirthDate = dto.BirthDate,
                HireDate = dto.HireDate,
                Address = dto.Address,
                City = dto.City,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                HomePhone = dto.HomePhone,
                Extension = dto.Extension,
                Photo = dto.Photo,
                Notes = dto.Notes,
                ReportsTo = dto.ReportsTo
            };

            _context.Employees.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDTO(entity);
        }

        public async Task<EmployeeDTO?> GetEmployeeById(long id)
        {
            string cacheKey = $"Employee_{id}";

            if (!_cache.TryGetValue(cacheKey, out EmployeeDTO? employeeDto))
            {
                employeeDto = await _context.Employees
                    .AsNoTracking()
                    .Where(e => e.Id == id)
                    .Select(e => MapToDTO(e))
                    .FirstOrDefaultAsync();

                if (employeeDto != null)
                {
                    _cache.Set(cacheKey, employeeDto, TimeSpan.FromMinutes(5));
                }
            }

            return employeeDto;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesPaged(int page, int pageSize)
        {
            string cacheKey = $"Employees_Page_{page}_Size_{pageSize}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<EmployeeDTO>? employees))
            {
                employees = await _context.Employees
                    .AsNoTracking()
                    .OrderBy(e => e.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => MapToDTO(e))
                    .ToListAsync();

                if (employees.Any())
                {
                    _cache.Set(cacheKey, employees, TimeSpan.FromMinutes(5));
                }
            }

            return employees ?? Enumerable.Empty<EmployeeDTO>();
        }


        public async Task<bool> UpdateEmployee(long id, EmployeeCreateDTO dto)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null) return false;

            entity.LastName = dto.LastName;
            entity.FirstName = dto.FirstName;
            entity.Title = dto.Title;
            entity.TitleOfCourtesy = dto.TitleOfCourtesy;
            entity.BirthDate = dto.BirthDate;
            entity.HireDate = dto.HireDate;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Region = dto.Region;
            entity.PostalCode = dto.PostalCode;
            entity.Country = dto.Country;
            entity.HomePhone = dto.HomePhone;
            entity.Extension = dto.Extension;
            entity.Photo = dto.Photo;
            entity.Notes = dto.Notes;
            entity.ReportsTo = dto.ReportsTo;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEmployee(long id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null) return false;

            _context.Employees.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static EmployeeDTO MapToDTO(Employee e) => new EmployeeDTO
        {
            Id = e.Id,
            LastName = e.LastName,
            FirstName = e.FirstName,
            Title = e.Title,
            TitleOfCourtesy = e.TitleOfCourtesy,
            BirthDate = e.BirthDate,
            HireDate = e.HireDate,
            Address = e.Address,
            City = e.City,
            Region = e.Region,
            PostalCode = e.PostalCode,
            Country = e.Country,
            HomePhone = e.HomePhone,
            Extension = e.Extension,
            Photo = e.Photo,
            Notes = e.Notes,
            ReportsTo = e.ReportsTo,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };

    }
}
