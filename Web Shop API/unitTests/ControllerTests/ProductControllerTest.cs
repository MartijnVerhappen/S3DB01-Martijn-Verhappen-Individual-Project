using Logic.IServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_API.Controllers;
using Web_Shop_API.IRepositories;
using Web_Shop_API.Models;

namespace unitTests.ControllerTests
{
    public class ProductControllerTest
    {
        [Fact]
        public void Test_Controller_GetAllProducts()
        {
            //Arrange
            var mockService = new Mock<IProductService>();

            var products = new List<ProductModel>
            {
            new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
            new ProductModel(2, "Videogame", "God of War", 50, 60)
            };

            mockService.Setup(service => service.GetAllProducts()).ReturnsAsync(products);

            var controller = new ProductController(mockService.Object);

            //Act
            var result = controller.GetAllProducts().Result;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Monster Hunter World", result[0].ProductNaam);
            Assert.Equal("God of War", result[1].ProductNaam);
        }
    }
}
