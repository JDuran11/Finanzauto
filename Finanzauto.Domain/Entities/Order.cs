using Finanzauto.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Domain.Entities
{
    [Table("Orders", Schema = "Store")]
    public class Order : BaseEntity
    {
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public long? ShipVia { get; set; }
        public virtual Shipper Shipper { get; set; }

        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
