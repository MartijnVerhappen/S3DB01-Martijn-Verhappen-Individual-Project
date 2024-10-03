using Logic.IServices;
using Web_Shop_API.IRepositories;
using Web_Shop_API.Models;

namespace Logic.Services
{
    public class ProductService : IProductService
    {
        public IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }
    }
}
