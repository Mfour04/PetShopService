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
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using WebRazor.Models.Services;
using WebRazor.Models.SystemModels;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

// define builder to manage service
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.
builder.Services.AddDbContext<PetShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnect"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
); // Thêm dòng này để đăng ký PetShopContext
																						   
// Add services to the container.
builder.Services.AddSingleton(payOS);

//Config Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
builder.Services.AddScoped<IServiceScheduleRepository, ServiceScheduleRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IShopServiceRepository, ShopServiceRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

//Config Service
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductOrderService>();
builder.Services.AddScoped<ServiceScheduleService>();
builder.Services.AddScoped<ShopServiceService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductCategoryService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// lấy thông tin HTTP request ở những lớp không thuộc controller
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Cấu hình Cookie Authentication cho Razor Pages
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Trang đăng nhập
        options.AccessDeniedPath = "/AccessDenied"; // Trang khi truy cập bị từ chối
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie
        options.SlidingExpiration = true; // Gia hạn thời gian tồn tại của cookie khi người dùng hoạt động
    });

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

builder.Services.AddAuthorization(options =>
{
	// Cấu hình phân quyền theo Role
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
	options.AddPolicy("UserOnly", policy => policy.RequireRole("user"));
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
app.MapHub<ChatHub>("/chathub");
app.Run();
