using MassTransit;

namespace OrdersApi.Consumers;

public class OrderCreatedConsumerDefinition : ConsumerDefinition<OrderCreatedConsumer>
{
    public OrderCreatedConsumerDefinition()
    {
        EndpointName = "order-created";
        Endpoint(e =>
        {
            e.Name = "order-created";
            e.ConcurrentMessageLimit = 10;
        });
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(r => r.Intervals(5, 10, 10, 10));
    }
}