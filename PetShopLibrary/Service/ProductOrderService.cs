using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Service
{
    public class ProductOrderService
    {
        private readonly IProductOrderRepository _orderRepository;
        

        public ProductOrderService(IProductOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddToCart(long productId, int quantity, long userId)
        {
            var order = new ProductOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ProductOrderDetails = new List<ProductOrderDetail>()
            };

            var orderDetail = new ProductOrderDetail
            {
                OrderId = order.OrderId,
                ProductId = productId,
            };

            order.ProductOrderDetails.Add(orderDetail);

            _orderRepository.AddOrder(order);
        }

        // Add order by user into database
        public long OrderProcessing(long userId)
        {
            var order = new ProductOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now
            };

           return _orderRepository.CreateNewOrder(order);
        }
    }
}
