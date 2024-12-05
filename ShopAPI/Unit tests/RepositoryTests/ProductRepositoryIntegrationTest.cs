using DAL;
using DAL.Repositories;
using Logic.IService;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopAPI.Controllers;

namespace Unit_tests.RepositoryTests
{
    [TestClass]
    public class ProductRepositoryIntegrationTest
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly DbContextOptions<DBContext> _options;

        public ProductRepositoryIntegrationTest()
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
        public async Task Test_Repository_GetAllProducts_HappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                ProductRepository repository = new ProductRepository(context);

                // Act
                List<Product> result = await repository.GetAllProducts();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("Monster Hunter World", result[0].ProductNaam);
                Assert.AreEqual("God of War", result[1].ProductNaam);
            }
        }

        [TestMethod]
        public async Task Test_Repository_GetAllProducts_UnhappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                ProductRepository repository = new ProductRepository(context);

                // Act
                List<Product> result = await repository.GetAllProducts();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task Test_Repository_UpdateProducts_HappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                var existingProduct = new Product(1, "Videogame", "Monster Hunter World", 60, 10);
                context.Product.Add(existingProduct);
                await context.SaveChangesAsync();

                ProductRepository repository = new ProductRepository(context);
                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 50, 20);

                // Act
                Product result = await repository.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual("Videogame", result.ProductType);
                Assert.AreEqual("Monster Hunter World", result.ProductNaam);
                Assert.AreEqual(50, result.ProductPrijs);
                Assert.AreEqual(20, result.ProductKorting);
            }
        }

        [TestMethod]
        public async Task Test_Repository_UpdateProducts_UnhappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                ProductRepository repository = new ProductRepository(context);
                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 50, 20);

                // Act
                Product result = await repository.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
