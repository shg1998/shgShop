using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(s => s.Id).HasConversion(
                s => s.Value,
                dbId => CustomerId.Of(dbId));
            builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Email).HasMaxLength(250);
            builder.HasIndex(s => s.Email).IsUnique();
        }
    }
}
