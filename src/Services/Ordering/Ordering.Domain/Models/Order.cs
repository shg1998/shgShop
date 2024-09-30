using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<Guid>
    {
        private readonly List<OrderItem> _orderItems = [];
        public IReadOnlyList<OrderItem> OrderItems => this._orderItems.AsReadOnly();
        public Guid CustomerId { get; set; } = default!;
        public string OrderName { get; set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public decimal TotalPrice
        {
            get => this.OrderItems.Sum(s => s.Price * s.Quantity);
            private set { }
        }
    }
}
