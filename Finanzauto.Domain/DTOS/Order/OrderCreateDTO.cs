using Finanzauto.Domain.DTOS.OrderDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Domain.DTOS.Order
{
    public class OrderCreateDTO
    {
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ID del cliente debe ser mayor a 0")]
        public long CustomerId { get; set; }

        [Required(ErrorMessage = "El ID del empleado es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ID del empleado debe ser mayor a 0")]
        public long EmployeeId { get; set; }

        public DateTime? RequiredDate { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "El ID del transportista debe ser mayor a 0")]
        public long? ShipVia { get; set; }

        [Range(0, 999999.99, ErrorMessage = "El costo de envío debe estar entre 0 y 999999.99")]
        public decimal? Freight { get; set; }

        [StringLength(200, ErrorMessage = "El nombre de envío no puede exceder 200 caracteres")]
        public string? ShipName { get; set; }

        [StringLength(300, ErrorMessage = "La dirección de envío no puede exceder 300 caracteres")]
        public string? ShipAddress { get; set; }

        [StringLength(100, ErrorMessage = "La ciudad de envío no puede exceder 100 caracteres")]
        public string? ShipCity { get; set; }

        [StringLength(100, ErrorMessage = "La región de envío no puede exceder 100 caracteres")]
        public string? ShipRegion { get; set; }

        [StringLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres")]
        [RegularExpression(@"^[0-9A-Za-z\s\-]+$", ErrorMessage = "El código postal contiene caracteres inválidos")]
        public string? ShipPostalCode { get; set; }

        [StringLength(100, ErrorMessage = "El país de envío no puede exceder 100 caracteres")]
        public string? ShipCountry { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un detalle de orden")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle de orden")]
        public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new();
    }
}
