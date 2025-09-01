using Finanzauto.Domain.DTOS.Customer;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finanzauto.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public CustomerRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<CustomerDTO> CreateCustomer(CustomerCreateDTO dto)
        {
            var entity = new Customer
            {
                CompanyName = dto.CompanyName,
                ContactName = dto.ContactName,
                ContactTitle = dto.ContactTitle,
                Address = dto.Address,
                City = dto.City,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Fax = dto.Fax
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();

            return new CustomerDTO
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                ContactName = entity.ContactName,
                ContactTitle = entity.ContactTitle,
                Address = entity.Address,
                City = entity.City,
                Region = entity.Region,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
                Phone = entity.Phone,
                Fax = entity.Fax,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<CustomerDTO?> GetCustomerById(long id)
        {
            string cacheKey = $"Customer_{id}";

            if (!_cache.TryGetValue(cacheKey, out CustomerDTO? customerDto))
            {
                customerDto = await _context.Customers
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new CustomerDTO
                    {
                        Id = c.Id,
                        CompanyName = c.CompanyName,
                        ContactName = c.ContactName,
                        ContactTitle = c.ContactTitle,
                        Address = c.Address,
                        City = c.City,
                        Region = c.Region,
                        PostalCode = c.PostalCode,
                        Country = c.Country,
                        Phone = c.Phone,
                        Fax = c.Fax,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (customerDto != null)
                {
                    _cache.Set(cacheKey, customerDto, TimeSpan.FromMinutes(5));
                }
            }

            return customerDto;
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomerPaged(int page, int pageSize)
        {
            string cacheKey = $"Customers_Page_{page}_Size_{pageSize}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<CustomerDTO>? customers))
            {
                customers = await _context.Customers
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CustomerDTO
                    {
                        Id = c.Id,
                        CompanyName = c.CompanyName,
                        ContactName = c.ContactName,
                        ContactTitle = c.ContactTitle,
                        Address = c.Address,
                        City = c.City,
                        Region = c.Region,
                        PostalCode = c.PostalCode,
                        Country = c.Country,
                        Phone = c.Phone,
                        Fax = c.Fax,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync();

                if (customers.Any())
                {
                    _cache.Set(cacheKey, customers, TimeSpan.FromMinutes(5));
                }
            }

            return customers ?? Enumerable.Empty<CustomerDTO>();
        }

        public async Task<bool> UpdateCustomer(long id, CustomerCreateDTO dto)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return false;

            entity.CompanyName = dto.CompanyName;
            entity.ContactName = dto.ContactName;
            entity.ContactTitle = dto.ContactTitle;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Region = dto.Region;
            entity.PostalCode = dto.PostalCode;
            entity.Country = dto.Country;
            entity.Phone = dto.Phone;
            entity.Fax = dto.Fax;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCustomer(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return false;

            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
