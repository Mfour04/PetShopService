using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
	public interface IUserRepository
	{
		IEnumerable<User> GetUsers();

		User? GetUserById(long userId);
		void AddUser(User user);
		void UpdateUser(User user);
		void DeleteUser(long userId);
	}
}
