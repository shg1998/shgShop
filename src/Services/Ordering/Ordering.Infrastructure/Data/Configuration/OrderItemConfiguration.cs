using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasConversion(s => s.Value, dbId => OrderItemId.Of(dbId));
            builder.HasOne<Product>().WithMany().HasForeignKey(s => s.ProductId);
            builder.Property(s => s.Quantity).IsRequired();
            builder.Property(s => s.Price).IsRequired();

        }
    }
}
