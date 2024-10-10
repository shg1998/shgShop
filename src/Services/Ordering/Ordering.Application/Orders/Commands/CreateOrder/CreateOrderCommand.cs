using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

    public record CreateOrderResult(Guid Id);

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(s => s.Order.OrderName).NotEmpty().WithMessage("Name is required!");
            RuleFor(s => s.Order.CustomerId).NotNull().WithMessage("CustomerId is required!");
            RuleFor(s => s.Order.OrderItems).NotEmpty().WithMessage("OrderItems is required!");
        }
    }

}
