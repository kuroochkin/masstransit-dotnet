using Contracts.Responses;
using MassTransit;
using Orders.Service;

namespace OrdersApi.Consumers;

public class VerifyOrderConsumer(IOrderService orderService) : IConsumer<VerifyOrder>
{
    public async Task Consume(ConsumeContext<VerifyOrder> context)
    {
        var order = await orderService.GetOrderAsync(context.Message.Id);

        await context.RespondAsync<OrderResult>(new
        {
            context.Message.Id,
            order.OrderDate,
            order.Status
        });
    }
}