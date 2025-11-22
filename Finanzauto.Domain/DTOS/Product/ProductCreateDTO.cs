using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Domain.DTOS.Product
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El nombre del producto debe tener entre 1 y 200 caracteres")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "El ID del proveedor es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ID del proveedor debe ser mayor a 0")]
        public long SupplierId { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El ID de la categoría debe ser mayor a 0")]
        public long CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "La cantidad por unidad no puede exceder 100 caracteres")]
        public string? QuantityPerUnit { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "El precio unitario debe estar entre 0.01 y 999999.99")]
        public decimal? UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las unidades en stock no pueden ser negativas")]
        public int? UnitsInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las unidades en orden no pueden ser negativas")]
        public int? UnitsOnOrder { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El nivel de reorden no puede ser negativo")]
        public int? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
