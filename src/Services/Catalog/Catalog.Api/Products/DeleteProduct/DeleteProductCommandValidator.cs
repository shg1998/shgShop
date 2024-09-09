using FluentValidation;

namespace Catalog.Api.Products.DeleteProduct
{
    public class DeleteProductCommandValidator: AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(s => s.Id).NotEmpty().WithMessage("ID must not be empty!");
        }
    }
}
