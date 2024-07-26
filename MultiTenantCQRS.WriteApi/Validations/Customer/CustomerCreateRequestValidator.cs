using FluentValidation;
using MultiTenantCQRS.WriteApi.Transport.Request.Customer;
using MultiTenantCQRS.WriteApi.Validations.Order;

namespace MultiTenantCQRS.WriteApi.Validations.Customer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CustomerCreateRequest>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleForEach(x => x.Orders).SetValidator(new OrderCreateRequestValidator());
        }
    }
}
