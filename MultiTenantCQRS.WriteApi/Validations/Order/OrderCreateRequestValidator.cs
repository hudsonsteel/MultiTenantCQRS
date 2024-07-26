using FluentValidation;
using MultiTenantCQRS.WriteApi.Transport.Request.Order;

namespace MultiTenantCQRS.WriteApi.Validations.Order
{
    public class OrderCreateRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public OrderCreateRequestValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Order date is required.");
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
            RuleFor(x => x.CustomerId).GreaterThan(0);
        }
    }
}
