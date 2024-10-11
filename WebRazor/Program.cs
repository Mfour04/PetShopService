
using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Service;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<PetShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetShopDatabase")));

// Đăng ký các dịch vụ khác
builder.Services.AddScoped<IServiceScheduleRepository, ServiceScheduleRepository>();
builder.Services.AddScoped<ServiceScheduleService>();

// Add services to the container.
builder.Services.AddRazorPages();


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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
