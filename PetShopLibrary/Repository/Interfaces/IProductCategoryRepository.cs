using PetShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Repository.Interfaces
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategoryById(long id);
        void AddProductCategory(ProductCategory productCategory);
    }
}
