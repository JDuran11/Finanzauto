using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Order;
using Finanzauto.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository repository, ILogger<OrderService> logger)
        {
            _orderRepository = repository;
            _logger = logger;
        }

        public async Task<OrderDTO?> GetOrderById(long id)
        {
            try
            {
                return await _orderRepository.GetOrderById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la orden con Id {OrderId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersPaged(int page, int pageSize)
        {
            try
            {
                return await _orderRepository.GetOrdersPaged(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las órdenes (Page: {Page}, PageSize: {PageSize})", page, pageSize);
                return Enumerable.Empty<OrderDTO>();
            }
        }

        public async Task<OrderDTO?> CreateOrder(OrderCreateDTO dto)
        {
            try
            {
                return await _orderRepository.CreateOrder(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la orden para el cliente {CustomerId}", dto.CustomerId);
                return null;
            }
        }

        public async Task<bool> UpdateOrder(long id, OrderCreateDTO dto)
        {
            try
            {
                return await _orderRepository.UpdateOrder(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la orden con Id {OrderId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteOrder(long id)
        {
            try
            {
                return await _orderRepository.DeleteOrder(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la orden con Id {OrderId}", id);
                return false;
            }
        }
    }
}
