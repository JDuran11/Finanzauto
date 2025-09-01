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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.CompanyName);
            builder.HasIndex(c => c.City);
            builder.HasIndex(c => c.Region);
            builder.HasIndex(c => c.PostalCode);
        }
    }
}
