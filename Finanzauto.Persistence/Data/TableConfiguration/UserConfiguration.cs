using Finanzauto.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Persistence.Data.TableConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasData(new User
            {
                Id = 1,
                UserName = "admin",
                Email = "jhonatandzm@gmail.com",
                Password = "$2a$11$WpD4j/12PVU8HC4ejmU5..UOgzal.B3kstTrbTaMzr.cVM0C84FC2", //admin
                CreatedAt = new DateTime(2025, 8, 29, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 8, 29, 10, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}
