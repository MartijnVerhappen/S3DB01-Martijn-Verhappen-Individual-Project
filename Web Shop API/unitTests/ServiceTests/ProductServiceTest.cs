using Logic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_API.DAL.IRepositories;
using Web_Shop_API.Models;

namespace unitTests.ServiceTests
{
    public class ProductServiceTest
    {
        [Fact]
        public void Test_Service_GetAllProducts()
        {
            //Arrange
            var mockRepo = new Mock<IProductRepository>();

            var products = new List<ProductModel>
            {
            new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
            new ProductModel(2, "Videogame", "God of War", 50, 60)
            };

            mockRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

            var service = new ProductService(mockRepo.Object);

            //Act
            var result = service.GetAllProducts().Result;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Monster Hunter World", result[0].ProductNaam);
            Assert.Equal("God of War", result[1].ProductNaam);
        }
    }
}
