using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;

namespace PetShopLibrary.Service
{
    
    public class DemoService
    {
        private IDemoRepository _demoRepository = new DemoRepository();

        public void addDemo(DemoModel demo)
        {
            _demoRepository.addDemo(demo); // goi // gọi repo để xử lý request từ controller
        }
    }
}
