using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<ShopService>> GetAllServicesAsync();
        Task<ShopService> GetServiceByIdAsync(long serviceId);
    }
}
