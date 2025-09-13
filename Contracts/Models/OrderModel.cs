﻿using Orders.Domain.Entities;

namespace Contracts.Models
{
    public class OrderModel
    {

        public OrderModel()
        {
            Status = OrderStatus.Created;
            OrderId = Guid.NewGuid();

        }
        private OrderStatus Status { get; set; }

        //customer things
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Guid OrderId { get; set; }
       
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
