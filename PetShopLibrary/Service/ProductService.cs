﻿using PetShopLibrary.Models;
using PetShopLibrary.Repository.Implements;
using PetShopLibrary.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopLibrary.Service
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
        public Product? GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }
        public void AddProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }
        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }
        public void DeleteProduct(long productId)
        {
            _productRepository.DeleteProduct(productId);
        }
    }
}
