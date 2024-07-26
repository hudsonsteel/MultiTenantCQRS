namespace MultiTenantCQRS.Write.Domain.Interfaces.Bus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}
