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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {

            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Order)
                .WithMany(o => o.OrderDetails)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(o => o.OrderId);

            builder.HasOne(o => o.Product)
                .WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(o => o.ProductId);
        }
    }
}
