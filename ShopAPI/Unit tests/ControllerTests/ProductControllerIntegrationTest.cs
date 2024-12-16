using DAL;
using DAL.Entities;
using DAL.Repositories;
using Logic.CustomExceptions;
using Logic.IService;
using Logic.Models;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopAPI.Controllers;

namespace Unit_tests.ControllerTests
{
    [TestClass]
    public class ProductControllerIntegrationTest
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly DbContextOptions<DBContext> _options;

        public ProductControllerIntegrationTest()
        {
            _options = new DbContextOptionsBuilder<DBContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            using (DBContext context = new DBContext(_options))
            {
                context.Database.EnsureCreated();
            }

            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
        }

        private async Task SeedDatabase(DBContext context)
        {
            context.Product.AddRange(
                new ProductEntity(1, "Videogame", "Monster Hunter World", 60, 0),
                new ProductEntity(2, "Videogame", "God of War", 50, 60)
            );
            await context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_Controller_GetAllProducts_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                ProductRepository repository = new ProductRepository(context);
                ProductService service = new ProductService(repository);
                ProductController controller = new ProductController(service);

                // Act
                var result = await controller.GetAllProducts();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("Monster Hunter World", result[0].ProductNaam);
                Assert.AreEqual("God of War", result[1].ProductNaam);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetAllProducts_UnhappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                ProductRepository repository = new ProductRepository(context);
                ProductService service = new ProductService(repository);
                ProductController controller = new ProductController(service);

                // Act
                var result = await controller.GetAllProducts();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task Test_Controller_UpdateProduct_HappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                // Seed the database with a product
                ProductEntity existingProduct = new ProductEntity(1, "Videogame", "Monster Hunter World", 60, 10);
                context.Product.Add(existingProduct);
                await context.SaveChangesAsync();

                var repository = new ProductRepository(context);
                var service = new ProductService(repository);
                var controller = new ProductController(service);

                // Update the existing product
                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 50, 20);

                // Act
                Product result = await controller.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(updatedProduct.Id, result.Id);
                Assert.AreEqual(updatedProduct.ProductType, result.ProductType);
                Assert.AreEqual(updatedProduct.ProductNaam, result.ProductNaam);
                Assert.AreEqual(updatedProduct.ProductPrijs, result.ProductPrijs);
                Assert.AreEqual(updatedProduct.ProductKorting, result.ProductKorting);
            }
        }

        [TestMethod]
        public async Task Test_Controller_UpdateProduct_UnhappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                var repository = new ProductRepository(context);
                var service = new ProductService(repository);
                var controller = new ProductController(service);

                // Update a non-existent product
                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 50, 20);

                // Act and Assert
                await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () => await controller.UpdateProduct(updatedProduct));
            }
        }

    }
}
