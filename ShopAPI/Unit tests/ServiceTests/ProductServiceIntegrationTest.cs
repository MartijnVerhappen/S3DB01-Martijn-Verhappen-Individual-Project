using DAL;
using DAL.Repositories;
using Logic.Models;
using Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace Unit_tests.ServiceTests
{
    [TestClass]
    public class ProductServiceIntegrationTest
    {
        private ProductService _service;
        private DbContextOptions<DBContext> _options;

        public ProductServiceIntegrationTest()
        {
            _options = new DbContextOptionsBuilder<DBContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            using (var context = new DBContext(_options))
            {
                context.Database.EnsureCreated();
            }

            var repository = new ProductRepository(new DBContext(_options));
            _service = new ProductService(repository);
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
                await SeedDatabase(context);
                ProductService service = new ProductService(new ProductRepository(context));
                Product updatedProduct = new Product(1, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                Product result = await service.UpdateProduct(updatedProduct);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual("Videogame", result.ProductType);
                Assert.AreEqual("Monster Hunter World", result.ProductNaam);
                Assert.AreEqual(60, result.ProductPrijs);
                Assert.AreEqual(10, result.ProductKorting);
            }
        }

        [TestMethod]
        public async Task Test_Repository_UpdateProducts_UnhappyFlow()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                ProductService service = new ProductService(new ProductRepository(context));
                int nonExistentProductId = 999;
                Product updatedProduct = new Product(nonExistentProductId, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                try
                {
                    await service.UpdateProduct(updatedProduct);
                    Assert.Fail("Expected an exception to be thrown.");
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.IsInstanceOfType(ex, typeof(Exception));
                    Assert.AreEqual("Product not found", ex.Message);
                }
            }
        }

    }
}
