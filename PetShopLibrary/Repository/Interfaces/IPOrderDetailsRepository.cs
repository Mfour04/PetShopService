using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IPOrderDetailsRepository
    {
        void AddRangePOrderDetails(List<ProductOrderDetail> productOrderDetails);
    }
}
