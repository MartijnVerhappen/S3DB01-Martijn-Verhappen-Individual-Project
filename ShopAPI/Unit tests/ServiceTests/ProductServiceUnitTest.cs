using Logic.IRepositories;
using Logic.IService;
using Logic.Models;
using Logic.Services;
using Moq;

namespace Unit_tests.ServiceTests
{
    [TestClass]
    public class ProductServiceUnitTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductService _productService;

        public ProductServiceUnitTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [TestMethod]
        public async Task GetAllProducts_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product(1, "Type A", "Product A", 10.99, 20),
                new Product(2, "Type B", "Product B", 15.99, 10)
            };

            _mockProductRepository.Setup(repo => repo.GetAllProducts())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.AreEqual(products, result);
        }

        [TestMethod]
        public async Task GetProductById_ReturnsProduct()
        {
            // Arrange
            var product = new Product(1, "Type A", "Product A", 10.99, 20);

            _mockProductRepository.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductById(1);

            // Assert
            Assert.AreEqual(product, result);
        }

        [TestMethod]
        public async Task UpdateProduct_UpdatesExistingProduct()
        {
            // Arrange
            var existingProduct = new Product(1, "Type A", "Product A", 10.99, 20);
            var updatedProduct = new Product(1, "Type B", "Product B", 15.99, 10);

            _mockProductRepository.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productService.UpdateProduct(updatedProduct);

            // Assert
            Assert.AreEqual(updatedProduct.ProductType, result.ProductType);
            Assert.AreEqual(updatedProduct.ProductNaam, result.ProductNaam);
            Assert.AreEqual(updatedProduct.ProductPrijs, result.ProductPrijs);
            Assert.AreEqual(updatedProduct.ProductKorting, result.ProductKorting);
        }

        [TestMethod]
        public async Task AddProduct_AddsNewProduct()
        {
            // Arrange
            var newProduct = new Product(1, "Type A", "Product A", 10.99, 20);

            _mockProductRepository.Setup(repo => repo.AddProduct(newProduct))
                .ReturnsAsync(newProduct);

            // Act
            var result = await _productService.AddProduct(newProduct);

            // Assert
            Assert.AreEqual(newProduct, result);
        }

        [TestMethod]
        public async Task UpdateProduct_ThrowsExceptionWhenProductNotFound()
        {
            // Arrange
            var updatedProduct = new Product(1, "Type B", "Product B", 15.99, 10);

            _mockProductRepository.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync((Product)null);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _productService.UpdateProduct(updatedProduct));
        }
    }
}
