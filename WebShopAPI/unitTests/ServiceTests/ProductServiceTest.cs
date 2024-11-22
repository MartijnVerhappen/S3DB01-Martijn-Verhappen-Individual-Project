using Logic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Core.DTO_s;
using DAL;
using Web_Shop_API.Repositories;

namespace unitTests.ServiceTests
{
    public class ProductServiceTest
    {
        private ProductService _service;
        private DbContextOptions<DBContext> _options;

        public ProductServiceTest()
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
                new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
                new ProductModel(2, "Videogame", "God of War", 50, 60)
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task Test_Service_GetAllProducts()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                // Act
                var result = await _service.GetAllProducts();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Monster Hunter World", result[0].ProductNaam);
                Assert.Equal("God of War", result[1].ProductNaam);
            }
        }

        [Fact]
        public async Task Test_Service_UpdateProduct()
        {
            using (var context = new DBContext(_options))
            {
                ProductDTO updatedProduct = new ProductDTO(1, "Videogame", "Monster Hunter World", 60, 10);
                await SeedDatabase(context);

                // Act
                ProductModel result = await _service.UpdateProduct(1, updatedProduct);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal(10, result.ProductKorting);
            }
        }
    }
}
