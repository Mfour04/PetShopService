using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Service
{
    public class POrderDetailsService
    {
        private readonly IPOrderDetailsRepository _pOrderDetailsRepository;
        public POrderDetailsService(IPOrderDetailsRepository pOrderDetailsRepository)
        {
            _pOrderDetailsRepository = pOrderDetailsRepository; 
        }
        public void AddRangePOrderDetails(List<ProductOrderDetail> productOrderDetails)
        {
            _pOrderDetailsRepository.AddRangePOrderDetails(productOrderDetails);
        }
    }
}
