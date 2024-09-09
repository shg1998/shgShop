using FluentValidation;

namespace Catalog.Api.Products.CreateProduct
{
    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is Required");
            RuleFor(s => s.Categories).NotEmpty().WithMessage("Categories is Required");
            RuleFor(s => s.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
            RuleFor(s => s.Price).GreaterThan(0).WithMessage("Name is Required");
        }
    }
}
