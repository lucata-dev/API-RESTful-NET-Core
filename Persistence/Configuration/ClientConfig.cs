using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(c => c.LastName)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(c => c.Birthday)
                   .IsRequired();

            builder.Property(c => c.Phone)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(c => c.Email)
                   .HasMaxLength(100);

            builder.Property(c => c.Address)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.Property(c => c.Age);

            builder.Property(c => c.CreatedBy)
                   .HasMaxLength(30);

            builder.Property(c => c.LastModifiedBy)
                   .HasMaxLength(30);
        }
    }
}
