namespace MultiTenantCQRS.Read.RabbitMq.Options
{
    public sealed record class RabbitMqOption
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public string VirtualHost { get; init; }
        public string QueueName { get; init; }
        public string ExchangeName { get; init; }
        public string RoutingKey { get; init; }
    }
}
