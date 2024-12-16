using Logic.CustomExceptions;
using Logic.IRepositories;
using Logic.IService;
using Logic.Models;

namespace Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var updatedProduct = await _productRepository.UpdateProduct(product);
            if(updatedProduct == null)
            {
                throw new DomainNotFoundException();
            }
            return await _productRepository.UpdateProduct(product);
        }

        public async Task<Product> AddProduct(Product product)
        {
            // Validate the product data
            if (string.IsNullOrEmpty(product.ProductType) || string.IsNullOrEmpty(product.ProductNaam) || product.ProductPrijs <= 0 || product.ProductKorting < 0 || product.ProductKorting > 100)
            {
                throw new Exception("Invalid product data");
            }

            return await _productRepository.AddProduct(product);
        }
    }
}
