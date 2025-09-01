using Finanzauto.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finanzauto.Domain.Entities
{
    [Table("Categories", Schema = "Store")]
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
