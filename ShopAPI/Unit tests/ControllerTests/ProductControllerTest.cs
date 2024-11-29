using DAL;
using DAL.Repositories;
using Logic.IService;
using Logic.Models;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopAPI.Controllers;

namespace Unit_tests.ControllerTests
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly DbContextOptions<DBContext> _options;

        public ProductControllerTest()
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
                new Product(1, "Videogame", "Monster Hunter World", 60, 0),
                new Product(2, "Videogame", "God of War", 50, 60)
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
                await SeedDatabase(context);

                var repository = new ProductRepository(context);
                var service = new ProductService(repository);
                var controller = new ProductController(service);

                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                Product result = await controller.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(updatedProduct, result);
                Assert.AreEqual(10, result.ProductKorting);
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

                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                Product result = await controller.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
