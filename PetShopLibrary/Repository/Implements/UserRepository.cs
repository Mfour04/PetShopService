using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
	public class UserRepository : IUserRepository
	{

		private readonly PetShopContext _context;
		public UserRepository(PetShopContext context)
		{
			_context = context;
		}
		public void AddUser(User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public void DeleteUser(long userId)
		{
			var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
			if (user != null)
			{
				_context.Users.Remove(user);
				_context.SaveChanges();
			}
		}

		public User? GetUserById(long userId)
		{
			return _context.Users.FirstOrDefault(u => u.UserId == userId);
		}

		public IEnumerable<User> GetUsers()
		{
			return _context.Users.ToList();
		}

		public void UpdateUser(User user)
		{
			_context.Users.Update(user);
			_context.SaveChanges();
		}
	}
}
