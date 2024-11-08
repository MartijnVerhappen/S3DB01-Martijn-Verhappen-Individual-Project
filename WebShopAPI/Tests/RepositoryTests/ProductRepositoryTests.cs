using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Web_Shop_API.Models;
using Web_Shop_API.Repositories;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        [Test]
        public void Test_GetAllProducts()
        {
            // Arrange
            var products = new List<ProductModel>
            {
                new ProductModel(1, "Videogame", "Monster Hunter World", 60, 0),
                new ProductModel(2, "Videogame", "God of War", 50, 60)
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ProductModel>>();
            mockSet.As<IQueryable<ProductModel>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<ProductModel>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<ProductModel>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<ProductModel>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<DBContext>(); // Use your actual DbContext type
            mockContext.Setup(c => c.Product).Returns(mockSet.Object); // Ensure this matches your DbSet

            var repository = new ProductRepository(mockContext.Object);

            // Act
            List<ProductModel> result = repository.GetAllProducts().Result;

            // Assert
            ClassicAssert.NotNull(result);
            Assert.Equals(2, result.Count); // Check if two products are returned
            Assert.Equals("Monster Hunter World", result[0].ProductNaam); // Validate the first product
            Assert.Equals("God of War", result[1].ProductNaam); // Validate the second product
        }
    }
}
