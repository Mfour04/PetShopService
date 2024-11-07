using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
	public class ShopServiceRepository : IShopServiceRepository
	{
		private readonly PetShopContext _context;

		public ShopServiceRepository(PetShopContext context) { _context = context; }
		public void AddShopService(ShopService shopService)
		{
			_context.ShopServices.Add(shopService);
			_context.SaveChanges();
		}

		public void DeleteShopService(int id)
		{
			var shopService = _context.ShopServices.FirstOrDefault(s => s.ServiceId == id);
			if (shopService != null)
			{
				_context.ShopServices.Remove(shopService);
				_context.SaveChanges();
			}
		}

		public ShopService? GetShopServiceById(int id)
		{
			return _context.ShopServices.FirstOrDefault(s => s.ServiceId == id);
		}

		public IEnumerable<ShopService> GetShopServices()
		{
			return _context.ShopServices.ToList();
		}

		public void UpdateShopService(ShopService shopService)
		{
			_context.ShopServices.Update(shopService);
			_context.SaveChanges();
		}
	}
}
