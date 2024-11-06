using Core.IServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_API.Controllers;
using Core.Models;
using Core.DTO_s;
using Microsoft.EntityFrameworkCore;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Logic.Services;
using Web_Shop_API.Repositories;

namespace unitTests.ControllerTests
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

            using (var context = new DBContext(_options))
            {
                context.Database.EnsureCreated();
            }

            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
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
        public async Task Test_Controller_GetAllProducts()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var repository = new ProductRepository(context);
                var service = new ProductService(repository);
                var controller = new ProductController(service);

                // Act
                var result = await controller.GetAllProducts();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Monster Hunter World", result[0].ProductNaam);
                Assert.Equal("God of War", result[1].ProductNaam);
            }
        }

        [Fact]
        public async Task Test_Controller_UpdateProduct()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var repository = new ProductRepository(context);
                var service = new ProductService(repository);
                var controller = new ProductController(service);

                ProductDTO updatedProduct = new ProductDTO(1, "Videogame", "Monster Hunter World", 60, 10);

                // Act
                IActionResult result = await controller.UpdateProduct(1, updatedProduct);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedProduct = Assert.IsType<ProductModel>(okResult.Value);

                // Assert the product's fields
                Assert.NotNull(returnedProduct);
                Assert.Equal(1, returnedProduct.Id);
                Assert.Equal(10, returnedProduct.ProductKorting);
            }
        }
    }
}
