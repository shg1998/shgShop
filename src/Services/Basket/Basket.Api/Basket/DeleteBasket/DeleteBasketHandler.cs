using Basket.Api.Data;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator() => 
            RuleFor(s => s.Username).NotEmpty().WithMessage("Username must not be empty");
    }
    public class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            await repository.DeleteBasket(command.Username, cancellationToken);
            return new DeleteBasketResult(true);
        }
    }
}
