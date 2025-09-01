using Finanzauto.Domain.DTOS.Supplier;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public SupplierRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<SupplierDTO> CreateSupplier(SupplierCreateDTO dto)
        {
            var entity = new Supplier
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
                Fax = dto.Fax,
                HomePage = dto.HomePage
            };

            _context.Suppliers.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDTO(entity);
        }

        public async Task<SupplierDTO?> GetSupplierById(long id)
        {
            string cacheKey = $"Supplier_{id}";

            if (!_cache.TryGetValue(cacheKey, out SupplierDTO? supplierDto))
            {
                supplierDto = await _context.Suppliers
                    .AsNoTracking()
                    .Where(s => s.Id == id)
                    .Select(s => MapToDTO(s))
                    .FirstOrDefaultAsync();

                if (supplierDto != null)
                {
                    _cache.Set(cacheKey, supplierDto, TimeSpan.FromMinutes(5));
                }
            }

            return supplierDto;
        }

        public async Task<IEnumerable<SupplierDTO>> GetSuppliersPaged(int page, int pageSize)
        {
            string cacheKey = $"Suppliers_Page_{page}_Size_{pageSize}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<SupplierDTO>? suppliers))
            {
                suppliers = await _context.Suppliers
                    .AsNoTracking()
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => MapToDTO(s))
                    .ToListAsync();

                if (suppliers.Any())
                {
                    _cache.Set(cacheKey, suppliers, TimeSpan.FromMinutes(5));
                }
            }

            return suppliers ?? Enumerable.Empty<SupplierDTO>();
        }

        public async Task<bool> UpdateSupplier(long id, SupplierCreateDTO dto)
        {
            var entity = await _context.Suppliers.FindAsync(id);
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
            entity.HomePage = dto.HomePage;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSupplier(long id)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null) return false;

            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static SupplierDTO MapToDTO(Supplier s) => new SupplierDTO
        {
            Id = s.Id,
            CompanyName = s.CompanyName,
            ContactName = s.ContactName,
            ContactTitle = s.ContactTitle,
            Address = s.Address,
            City = s.City,
            Region = s.Region,
            PostalCode = s.PostalCode,
            Country = s.Country,
            Phone = s.Phone,
            Fax = s.Fax,
            HomePage = s.HomePage,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }
}
