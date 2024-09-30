using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetShopLibrary.Models;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IDemoRepository
    {
        public void addDemo(DemoModel demo);
    }
}
