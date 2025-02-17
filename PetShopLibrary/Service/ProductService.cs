using Microsoft.AspNetCore.Http;
using PetShopLibrary.DTO;
using PetShopLibrary.Models;
using PetShopLibrary.Repository.Interfaces;
using PetShopLibrary.Utility;

namespace PetShopLibrary.Service
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly CloudinaryService _cloudinaryService;

        public ProductService(IProductRepository productRepository, CloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
        public Product? GetProductById(long id)
        {
            return _productRepository.GetProductById(id);
        }
        public async Task AddProduct(ProductDTO productDTO, IFormFile? file)
        {
            if (file != null)
            {
                try
                {
                    var fileUrl = await _cloudinaryService.UploadImageAsync(file);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        productDTO.FilePath = fileUrl;
                    }
                    else
                    {
                        throw new Exception("Không thể nhận được URL ảnh từ Cloudinary.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi upload ảnh: " + ex.Message);
                }
            }

            var product = new Product
            {
                ProductName = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CategoryId = productDTO.CategoryId,
                FilePath = productDTO.FilePath
            };

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
        public async Task<PagedResult<Product>> GetPagedProductsAsync(int pageIndex, int pageSize, string searchText, decimal minPrice, decimal maxPrice)
        {
            return await _productRepository.GetProductsPagedAsync(pageIndex, pageSize, searchText, minPrice, maxPrice);
        }
        public IEnumerable<Product> SearchProducts(string keyword)
        {
            return _productRepository.SearchProduct(keyword);
        }

    }
}
