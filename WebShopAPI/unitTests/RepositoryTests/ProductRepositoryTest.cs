using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Core.Models;
using Web_Shop_API.Repositories;
using MySqlX.XDevAPI.Common;

namespace unitTests.RepositoryTests
{
    public class ProductRepositoryTest
    {

        private DbContextOptions<DBContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedDatabase(DBContext context)
        {
            context.Product.AddRange(
                new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
                new ProductModel(2, "Videogame", "God of War", 50, 60)
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task Test_Repository_GetAllProducts()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new DBContext(options))
            {
                await SeedDatabase(context);
                ProductRepository repository = new ProductRepository(context);

                // Act
                List<ProductModel> result = await repository.GetAllProducts();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Monster Hunter World", result[0].ProductNaam);
                Assert.Equal("God of War", result[1].ProductNaam);
            }
        }

        [Fact]
        public async Task Test_Repository_UpdateProducts()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new DBContext(options))
            {
                await SeedDatabase(context);
                ProductRepository repository = new ProductRepository(context);
                ProductModel updatedProduct = new ProductModel(1, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                ProductModel result = await repository.UpdateProduct(updatedProduct);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Videogame", result.ProductType);
                Assert.Equal("Monster Hunter World", result.ProductNaam);
                Assert.Equal(60, result.ProductPrijs);
                Assert.Equal(10, result.ProductKorting);
            }
        }
    }
}