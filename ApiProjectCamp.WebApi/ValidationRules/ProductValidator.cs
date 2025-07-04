using ApiProjectCamp.WebApi.Entities;
using FluentValidation;

namespace ApiProjectCamp.WebApi.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Enter the product name");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Name must be min 2 characters");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name must be max 50 characters");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Enter the product price").GreaterThan(0).WithMessage("Price should be min 1").LessThan(1001).WithMessage("Price must be max 1000");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Enter the product description");
        }
    }
}
