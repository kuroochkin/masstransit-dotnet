using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        Console.WriteLine($"Order {context.Message.OrderId} created");
    }
}