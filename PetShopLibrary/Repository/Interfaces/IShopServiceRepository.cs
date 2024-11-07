using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
	public interface IShopServiceRepository
	{
		IEnumerable<ShopService> GetShopServices();

		ShopService GetShopServiceById(int id);

		void AddShopService(ShopService shopService);
		void UpdateShopService(ShopService shopService);
		void DeleteShopService(int id);
	}
}
