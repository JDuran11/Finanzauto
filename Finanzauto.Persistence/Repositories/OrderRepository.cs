using Finanzauto.Domain.DTOS.Order;
using Finanzauto.Domain.DTOS.OrderDetail;
using Finanzauto.Domain.Entities;
using Finanzauto.Persistence.Data;
using Finanzauto.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finanzauto.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public OrderRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<OrderDTO> CreateOrder(OrderCreateDTO dto)
        {
            var entity = new Order
            {
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId,
                ShipVia = dto.ShipVia,
                Freight = dto.Freight,
                ShipName = dto.ShipName,
                ShipAddress = dto.ShipAddress,
                ShipCity = dto.ShipCity,
                ShipRegion = dto.ShipRegion,
                ShipPostalCode = dto.ShipPostalCode,
                ShipCountry = dto.ShipCountry,
                OrderDetails = dto.OrderDetails.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                }).ToList()
            };

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDTO(entity);
        }

        public async Task<OrderDTO?> GetOrderById(long id)
        {
            string cacheKey = $"Order_{id}";

            if (_cache.TryGetValue(cacheKey, out OrderDTO cachedOrder))
            {
                return cachedOrder;
            }

            var entity = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (entity == null) return null;

            var dto = MapToDTO(entity);

            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(2));

            return dto;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersPaged(int page, int pageSize)
        {
            string cacheKey = $"Orders_Page_{page}_Size_{pageSize}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<OrderDTO> cachedOrders))
            {
                return cachedOrders;
            }

            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderDetails)
                .OrderBy(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => MapToDTO(o))
                .ToListAsync();

            _cache.Set(cacheKey, orders, TimeSpan.FromMinutes(1));

            return orders;
        }

        public async Task<bool> UpdateOrder(long id, OrderCreateDTO dto)
        {
            var entity = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (entity == null) return false;

            entity.CustomerId = dto.CustomerId;
            entity.EmployeeId = dto.EmployeeId;
            entity.ShipVia = dto.ShipVia;
            entity.Freight = dto.Freight;
            entity.ShipName = dto.ShipName;
            entity.ShipAddress = dto.ShipAddress;
            entity.ShipCity = dto.ShipCity;
            entity.ShipRegion = dto.ShipRegion;
            entity.ShipPostalCode = dto.ShipPostalCode;
            entity.ShipCountry = dto.ShipCountry;

            entity.OrderDetails.Clear();
            foreach (var d in dto.OrderDetails)
            {
                entity.OrderDetails.Add(new OrderDetail
                {
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                });
            }

            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrder(long id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity == null) return false;

            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static OrderDTO MapToDTO(Order o) => new OrderDTO
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            EmployeeId = o.EmployeeId,
            ShipVia = o.ShipVia,
            Freight = o.Freight,
            ShipName = o.ShipName,
            ShipAddress = o.ShipAddress,
            ShipCity = o.ShipCity,
            ShipRegion = o.ShipRegion,
            ShipPostalCode = o.ShipPostalCode,
            ShipCountry = o.ShipCountry,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
            OrderDetails = o.OrderDetails.Select(d => new OrderDetailDTO
            {
                Id = d.Id,
                ProductId = d.ProductId,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity,
                Discount = d.Discount
            }).ToList()
        };
    }
}
