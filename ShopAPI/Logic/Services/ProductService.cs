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
            var existingProduct = await _productRepository.GetProductById(product.Id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            // Apply the changes
            existingProduct.ProductType = !string.IsNullOrEmpty(product.ProductType) ? product.ProductType : existingProduct.ProductType;
            existingProduct.ProductNaam = !string.IsNullOrEmpty(product.ProductNaam) ? product.ProductNaam : existingProduct.ProductNaam;
            existingProduct.ProductPrijs = product.ProductPrijs > 0 ? product.ProductPrijs : existingProduct.ProductPrijs;
            existingProduct.ProductKorting = product.ProductKorting >= 0 && product.ProductKorting <= 100 ? product.ProductKorting : existingProduct.ProductKorting;

            await _productRepository.UpdateProduct(existingProduct);

            return existingProduct;
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
