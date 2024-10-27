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

        public ProductOrder GetProductOrderByID(long orderId)
        {
            return _petShopContext.ProductOrders
                       .Include(o => o.ProductOrderDetails)
                       .FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
