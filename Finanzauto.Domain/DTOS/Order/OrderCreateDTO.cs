using Finanzauto.Domain.DTOS.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Domain.DTOS.Order
{
    public class OrderCreateDTO
    {
        public long CustomerId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime? RequiredDate { get; set; }
        public long? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new();
    }
}
