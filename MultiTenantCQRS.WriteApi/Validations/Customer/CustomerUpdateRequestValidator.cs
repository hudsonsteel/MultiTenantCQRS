using FluentValidation;
using MultiTenantCQRS.WriteApi.Transport.Request.Customer;
using MultiTenantCQRS.WriteApi.Validations.Order;

namespace MultiTenantCQRS.WriteApi.Validations.Customer
{
    public class CustomerUpdateRequestValidator : AbstractValidator<CustomerUpdateRequest>
    {
        public CustomerUpdateRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Customer ID must be greater than 0.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleForEach(x => x.Orders).SetValidator(new UpdateOrderRequestValidator());
        }
    }
}
