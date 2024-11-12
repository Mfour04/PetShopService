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
    public class ServiceRepository : IServiceRepository
    {
        private readonly PetShopContext _context;

        public ServiceRepository(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShopService>> GetAllServicesAsync()
        {
            return await _context.ShopServices.ToListAsync();
        }

        public async Task<ShopService> GetServiceByIdAsync(long serviceId)
        {
            return await _context.ShopServices.FindAsync(serviceId);
        }
    }
}
