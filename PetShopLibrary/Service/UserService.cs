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
	public class UserService
	{
		private readonly IUserRepository _userRepository;

		public UserService (IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public void AddUser(User user)
		{
			_userRepository.AddUser(user);
		}

		public void Delete(long userId)
		{
			_userRepository.DeleteUser(userId);
		}

		public User? GetUserById(long userId)
		{
			return _userRepository.GetUserById(userId);
		}

		public IEnumerable<User> GetUsers()
		{
			return _userRepository.GetUsers();

		}

		public void UpdateUser(User user)
		{
			_userRepository.UpdateUser(user);
		}
	}
}
