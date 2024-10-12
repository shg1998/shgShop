using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool IsSuccess);

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(s => s.Order.Id).NotEmpty().WithMessage("Id is required!");
            RuleFor(s => s.Order.OrderName).NotEmpty().WithMessage("OrderName is required!");
            RuleFor(s => s.Order.CustomerId).NotNull().WithMessage("CustomerId is required!");
        }
    }
}
