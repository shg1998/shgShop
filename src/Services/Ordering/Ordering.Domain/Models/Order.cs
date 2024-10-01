using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = [];
        public IReadOnlyList<OrderItem> OrderItems => this._orderItems.AsReadOnly();
        public CustomerId CustomerId { get; set; } = default!;
        public OrderName OrderName { get; set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public decimal TotalPrice
        {
            get => this.OrderItems.Sum(s => s.Price * s.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress,
            Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {
            this.OrderName = orderName;
            this.ShippingAddress = shippingAddress;
            this.BillingAddress = billingAddress;
            this.Payment = payment;
            this.Status = status;

            this.AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            var orderItem = new OrderItem(Id, productId, quantity, price);
            this._orderItems.Add(orderItem);
        }

        public void Remove(ProductId productId)
        {
            var orderItem = this._orderItems.FirstOrDefault(s => s.ProductId == productId);
            if (orderItem != null) this._orderItems.Remove(orderItem);
        }
    }
}
