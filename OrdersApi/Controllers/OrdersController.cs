using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Service.Clients;

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(
        IOrderService orderService,
        IProductStockServiceClient productStockServiceClient,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        ISendEndpointProvider sendEndpointProvider) : ControllerBase
    {
        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel model)
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-command"));
            await sendEndpoint.Send(model,
                context =>
                {
                    context.Headers.Set("command-header", "OrderCreated");
                    context.TimeToLive = TimeSpan.FromHours(1);
                });
            
            return Accepted();
        }
        
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            try
            {
                await orderService.UpdateOrderAsync(order);
            }
            catch
            {
                if (!await orderService.OrderExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await orderService.GetOrdersAsync();
            return Ok(orders);
        }



        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
