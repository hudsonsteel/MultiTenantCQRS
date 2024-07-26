using MediatR;

namespace MultiTenantCQRS.Write.Application.Commands.Customer
{
    public sealed record class DeleteCustomerCommand(int CustomerId) : IRequest<Unit>
    {
        public int Id { get; init; } = CustomerId;
    }
}
