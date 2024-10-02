using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configuration
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasConversion(s => s.Value, dbId => OrderId.Of(dbId));
            builder.HasOne<Customer>().WithMany().HasForeignKey(s => s.CustomerId).IsRequired();
            builder.HasMany(s => s.OrderItems).WithOne().HasForeignKey(s => s.OrderId);
            builder.ComplexProperty(s => s.OrderName, nameBuilder =>
            {
                nameBuilder.Property(s => s.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.ComplexProperty(s => s.ShippingAddress, addrBuilder =>
            {
                addrBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addrBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addrBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(100);

                addrBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addrBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

                addrBuilder.Property(a => a.State)
                    .HasMaxLength(50);

                addrBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

            builder.ComplexProperty(s => s.BillingAddress, addrBuilder =>
            {
                addrBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addrBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addrBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(100);

                addrBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addrBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

                addrBuilder.Property(a => a.State)
                    .HasMaxLength(50);

                addrBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

            builder.ComplexProperty(s => s.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(a => a.CardName)
                    .HasMaxLength(50);

                paymentBuilder.Property(a => a.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();

                paymentBuilder.Property(a => a.Expiration)
                    .HasMaxLength(10);

                paymentBuilder.Property(a => a.CVV)
                    .HasMaxLength(3);

                paymentBuilder.Property(a => a.PaymentMethod);
            });

            builder.Property(s => s.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(s => s.ToString(), dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));
        }
    }
}
