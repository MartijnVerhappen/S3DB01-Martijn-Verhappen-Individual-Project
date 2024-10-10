using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Web_Shop_API.Models;
using Web_Shop_API.Repositories;

namespace unitTests.RepositoryTests
{
    public class ProductRepositoryTest
    {

        [Fact]
        public void Test_Repository_GetAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Set up in-memory database
            .Options;

            using (var context = new DBContext(options))
            {
                context.Product.AddRange(
                    new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
                    new ProductModel(2, "Videogame", "God of War", 50, 60)
                );
                context.SaveChanges();

                var repository = new ProductRepository(context);

                // Act
                var result = repository.GetAllProducts().Result;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count); // Check if two products are returned
                Assert.Equal("Monster Hunter World", result[0].ProductNaam); // Validate the first product
                Assert.Equal("God of War", result[1].ProductNaam); // Validate the second product
            }
        }
    }
}