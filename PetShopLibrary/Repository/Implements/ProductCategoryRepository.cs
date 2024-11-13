using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Implements
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly PetShopContext _context;

        public ProductCategoryRepository(PetShopContext context) { _context = context; }
        public void AddProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            _context.SaveChanges();
        }

        public IEnumerable<ProductCategory> GetProductCategories()
        {
            return _context.ProductCategories.ToList();
        }

        public ProductCategory? GetProductCategoryById(int id)
        {
            return _context.ProductCategories.FirstOrDefault(c => c.CategoryId == id);
        }
    }
}
