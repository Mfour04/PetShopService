using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetShopLibrary.Models;

namespace WebRazor.Models.AuthenticationModel
{
	public class JwtTokenHelper
	{
		public static string GenerateJwtToken(User user, IConfiguration configuration)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? string.Empty);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, user.Email),
					new Claim(ClaimTypes.Role, user.RoleId), // Sử dụng RoleId từ user
					new Claim("userId", user.UserId.ToString()), // Thêm userId claim
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token); // Trả về token dưới dạng chuỗi
		}
	}

}
