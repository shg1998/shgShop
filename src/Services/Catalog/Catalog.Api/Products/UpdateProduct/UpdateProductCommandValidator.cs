using FluentValidation;

namespace Catalog.Api.Products.UpdateProduct
{
    public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("ID must not be empty!");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(s => s.Price).GreaterThan(0).WithMessage("Name is Required");
        }
    }
}
