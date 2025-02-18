using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
    public class ProductOrderRepository : IProductOrderRepository
    {
        private readonly PetShopContext _petShopContext;
        public ProductOrderRepository(PetShopContext petShopContext) { 
            _petShopContext = petShopContext;
        }
        public void AddOrder(ProductOrder order)
        {
            _petShopContext.ProductOrders.Add(order);
            _petShopContext.SaveChanges();
        }

        public long CreateNewOrder(ProductOrder order)
        {
            _petShopContext.ProductOrders.Add(order);
            _petShopContext.SaveChanges();
            return order.OrderId;
        }

        public IEnumerable<ProductOrder> GetAllProductOrders()
        {
            return _petShopContext.ProductOrders.ToList();
        }

        public IEnumerable<ProductOrder>? GetOrderByUserId(long userId)
        {
            return _petShopContext.ProductOrders.ToList().Where(x => x.UserId == userId);
        }

        public ProductOrder GetProductOrderByID(long orderId)
        {
            return _petShopContext.ProductOrders
                       .Include(o => o.ProductOrderDetails)
                       .FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
