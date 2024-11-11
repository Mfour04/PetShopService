using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(long productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(long productId);
        Task<PagedResult<Product>> GetProductsPagedAsync(int pageIndex, int pageSize, string searchText, decimal minPrice, decimal maxPrice);
    }
}
