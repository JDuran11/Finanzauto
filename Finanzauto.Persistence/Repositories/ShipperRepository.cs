using Finanzauto.Domain.DTOS.Shipper;
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
    public class ShipperRepository : IShipperRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ShipperRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ShipperDTO> AddShipper(ShipperCreateDTO dto)
        {
            var shipper = new Shipper
            {
                CompanyName = dto.CompanyName,
                Phone = dto.Phone
            };

            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();

            return new ShipperDTO
            {
                Id = shipper.Id,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone,
                CreatedAt = shipper.CreatedAt,
                UpdatedAt = shipper.UpdatedAt
            };
        }

        public async Task<IEnumerable<ShipperDTO>> GetAllShippers()
        {
            string cacheKey = "AllShippers";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ShipperDTO>? shippers))
            {
                shippers = await _context.Shippers
                    .AsNoTracking()
                    .Select(s => new ShipperDTO
                    {
                        Id = s.Id,
                        CompanyName = s.CompanyName,
                        Phone = s.Phone,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt
                    })
                    .ToListAsync();

                if (shippers.Any())
                {
                    _cache.Set(cacheKey, shippers, TimeSpan.FromMinutes(10));
                }
            }

            return shippers ?? Enumerable.Empty<ShipperDTO>();
        }

        public async Task<ShipperDTO?> GetShipperById(long id)
        {
            string cacheKey = $"Shipper_{id}";

            if (!_cache.TryGetValue(cacheKey, out ShipperDTO? shipperDto))
            {
                var s = await _context.Shippers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (s == null) return null;

                shipperDto = new ShipperDTO
                {
                    Id = s.Id,
                    CompanyName = s.CompanyName,
                    Phone = s.Phone,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                };

                _cache.Set(cacheKey, shipperDto, TimeSpan.FromMinutes(10));
            }

            return shipperDto;
        }

        public async Task<ShipperDTO?> UpdateShipper(long id, ShipperCreateDTO dto)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return null;

            shipper.CompanyName = dto.CompanyName;
            shipper.Phone = dto.Phone;
            shipper.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ShipperDTO
            {
                Id = shipper.Id,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone,
                CreatedAt = shipper.CreatedAt,
                UpdatedAt = shipper.UpdatedAt
            };
        }

        public async Task<bool> DeleteShipper(long id)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null) return false;

            _context.Shippers.Remove(shipper);
            await _context.SaveChangesAsync();
            return true;
        }

    }


}
