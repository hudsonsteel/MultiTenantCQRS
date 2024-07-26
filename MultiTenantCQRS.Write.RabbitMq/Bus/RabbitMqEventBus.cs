using MassTransit;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;

namespace MultiTenantCQRS.Write.RabbitMq.Bus
{
    public sealed class RabbitMqEventBus(IBus bus) : IEventBus
    {
        private readonly IBus _bus = bus;

        public async Task PublishAsync<T>(T @event) where T : class
        {
            await _bus.Publish(@event);
        }
    }
}
