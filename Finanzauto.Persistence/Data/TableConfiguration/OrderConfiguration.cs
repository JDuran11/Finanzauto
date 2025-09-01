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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(o => o.Id);

            builder.HasIndex(o => o.OrderDate);
            builder.HasIndex(o => o.ShippedDate);
            builder.HasIndex(o => o.ShipPostalCode);

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(o => o.CustomerId);

            builder.HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(o => o.EmployeeId);

            builder.HasOne(o => o.Shipper)
                .WithMany(c => c.Orders)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(o => o.ShipVia);
        }
    }
}
