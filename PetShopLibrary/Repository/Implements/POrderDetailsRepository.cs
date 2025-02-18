using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
    public class POrderDetailsRepository : IPOrderDetailsRepository
    {
        private readonly PetShopContext _context;
        public POrderDetailsRepository(PetShopContext context)
        {
            _context = context;
        }

        public void AddRangePOrderDetails(List<ProductOrderDetail> productOrderDetails)
        {
            _context.ProductOrderDetails.AddRange(productOrderDetails);
            _context.SaveChanges();
        }
    }
}
