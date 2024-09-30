using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;

namespace PetShopLibrary.Repository.Implements
{
    public class DemoRepository : IDemoRepository
    {
        public void addDemo(DemoModel demo)
        { 
            throw new NotImplementedException(); // gọi database để xử lý dữ liệu (triển khai của interface Repository để xử lý hàm)
        }
    }
}
