using FluentValidation;
using MultiTenantCQRS.WriteApi.Transport.Request.Order;

namespace MultiTenantCQRS.WriteApi.Validations.Order
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Order ID must be greater than 0.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Order date is required.");
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId must be greater than zero.");
        }
    }
}
