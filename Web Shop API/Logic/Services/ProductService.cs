using Logic.IServices;
using Web_Shop_API.Models;
using Microsoft.AspNetCore.Mvc;
using Web_Shop_API.DAL.IRepositories;

namespace Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<ProductModel> UpdateProduct(ProductModel updatedProduct)
        {
            return await _productRepository.UpdateProduct(updatedProduct);
        }
    }
}
