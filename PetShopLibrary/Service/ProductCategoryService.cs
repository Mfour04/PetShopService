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
    public class ProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public IEnumerable<ProductCategory> GetProductCategories()
        {
            return _productCategoryRepository.GetProductCategories();
        }

        public ProductCategory? GetProductCategoryById(int id)
        {
            return _productCategoryRepository.GetProductCategoryById(id);
        }
    }
}
