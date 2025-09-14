using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrderCreation.Worker;

public class CreateOrderConsumer : IConsumer<OrderModel>
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public CreateOrderConsumer(IMapper mapper, IOrderService orderService)
    {
        _mapper = mapper;
        _orderService = orderService;
    }
    
    public async Task Consume(ConsumeContext<OrderModel> context)
    {
        Console.WriteLine($"I got a command to create an order: {context.Message}");
        
        var order = _mapper.Map<Order>(context.Message);
        var createdOrder = await _orderService.AddOrderAsync(order);
        
        Console.WriteLine($"Order: {createdOrder.OrderId} saved.");
        
        var notifyOrderCreated = context.Publish(new OrderCreated
        {
            CreatedAt = createdOrder.OrderDate,
            Id = createdOrder.Id,
            OrderId = createdOrder.OrderId,
            TotalAmount = createdOrder.OrderItems.Sum(i => i.Price * i.Quantity)
        });
        
        await Task.CompletedTask;
    }
}