using Finanzauto.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Data.TableConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.ProductName);

            builder.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(p => p.SupplierId);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
