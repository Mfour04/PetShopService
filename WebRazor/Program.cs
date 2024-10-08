using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PetShopContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Thêm dòng này để đăng ký PetShopContext
																						   // Add services to the container.
// Add services to the container.
builder.Services.AddSingleton(payOS);
//Config Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Config Service
builder.Services.AddScoped<ProductService>();

builder.Services.AddRazorPages();

// Cấu hình JWT
var key = Encoding.ASCII.GetBytes("UKlgtQBwfAyxUEE6JusllbQqo44K3gBAZWeT6d4U");

builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero // Để token hết hạn chính xác
		};
	});

builder.Services.AddAuthorization(options =>
{
	// Cấu hình phân quyền theo Role
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers(); // Thêm dòng này để map các route của API
app.Run();
