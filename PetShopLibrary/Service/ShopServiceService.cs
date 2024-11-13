using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Service
{
    public class ShopServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ShopServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task<IEnumerable<ShopService>> GetAllServicesAsync()
        {
            return await _serviceRepository.GetAllServicesAsync();
        }
        public async Task<ShopService> GetServiceByIdAsync(long serviceId)
        {
            return await _serviceRepository.GetServiceByIdAsync(serviceId);
        }
    }
}
