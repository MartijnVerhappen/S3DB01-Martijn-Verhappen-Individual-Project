using Web_Shop_API.Entities;
using Web_Shop_API.Repositories;

namespace Web_Shop_API.Services
{
    public class ProductService
    {
        public ProductRepository _productRepository;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductEntity> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
    }
}
