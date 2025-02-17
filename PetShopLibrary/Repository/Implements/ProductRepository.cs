using Microsoft.EntityFrameworkCore;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;

namespace PetShopLibrary.Repository.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly PetShopContext _context;

        public ProductRepository(PetShopContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products
                    .Include(c => c.Category)
                  .ToList();
        }

        public Product? GetProductById(long productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductOrderDetails)
                .FirstOrDefault(p => p.ProductId == productId);
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(long productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public async Task<PagedResult<Product>> GetProductsPagedAsync(int pageIndex, int pageSize, string searchText, decimal minPrice, decimal maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(it => it.ProductName.Contains(searchText.ToLower()));
            }

            query = query.Where(it => it.Price >= minPrice && it.Price <= maxPrice);

            var totalCount = await query.CountAsync();

            var products = await query
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return new PagedResult<Product>(products, totalCount, pageIndex, pageSize);
        }

        public IEnumerable<Product> SearchProduct(string keyword)
        {
            return _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductName.Contains(keyword) ||
                p.Price.ToString().Contains(keyword) ||
                p.Category.CategoryName.Contains(keyword))
                .ToList();
        }
    }
}
