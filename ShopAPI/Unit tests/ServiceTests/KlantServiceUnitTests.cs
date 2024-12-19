using Logic.CustomExceptions;
using Logic.IRepositories;
using Logic.IService;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Threading.Tasks;

namespace Unit_tests.ServiceTests
{
    [TestClass]
    public class KlantServiceUnitTest
    {
        private readonly Mock<IKlantRepository> _mockKlantRepository;
        private readonly IKlantService _klantService;

        public KlantServiceUnitTest()
        {
            _mockKlantRepository = new Mock<IKlantRepository>();
            _klantService = new KlantService(_mockKlantRepository.Object);
        }

        [TestMethod]
        public async Task CreateKlantAsync_CreatesNewKlant()
        {
            // Arrange
            var newKlant = new Klant { Id = 1, Gebruikersnaam = "testuser" };

            _mockKlantRepository.Setup(repo => repo.CreateAsync(newKlant))
                .ReturnsAsync(newKlant);

            // Act
            var result = await _klantService.CreateKlantAsync(newKlant);

            // Assert
            Assert.AreEqual(newKlant, result);
        }

        [TestMethod]
        public async Task GetKlantByIdAsync_ReturnsKlant()
        {
            // Arrange
            var klant = new Klant { Id = 1, Gebruikersnaam = "testuser" };

            _mockKlantRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(klant);

            // Act
            var result = await _klantService.GetKlantByIdAsync(1);

            // Assert
            Assert.AreEqual(klant, result);
        }

        [TestMethod]
        public async Task GetKlantByGebruikersnaamAsync_ReturnsKlant()
        {
            // Arrange
            var klant = new Klant { Id = 1, Gebruikersnaam = "testuser" };

            _mockKlantRepository.Setup(repo => repo.GetByUsernameAsync("testuser"))
                .ReturnsAsync(klant);

            // Act
            var result = await _klantService.GetKlantByUsernameAsync("testuser");

            // Assert
            Assert.AreEqual(klant, result);
        }

        [TestMethod]
        public async Task UpdateKlantAsync_UpdatesExistingKlant()
        {
            // Arrange
            var existingKlant = new Klant { Id = 1, Gebruikersnaam = "testuser" };
            var updatedKlant = new Klant { Id = 1, Gebruikersnaam = "updateduser" };

            _mockKlantRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingKlant);
            _mockKlantRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Klant>()))
                .ReturnsAsync(updatedKlant);

            // Act
            var result = await _klantService.UpdateKlantAsync(updatedKlant);

            // Assert
            Assert.AreEqual(updatedKlant.Gebruikersnaam, result.Gebruikersnaam);
        }

        [TestMethod]
        public async Task DeleteKlantAsync_CallsDeleteAsyncOnRepository()
        {
            // Arrange
            var klantId = 1;

            // Act
            await _klantService.DeleteKlantAsync(klantId);

            // Assert
            _mockKlantRepository.Verify(repo => repo.DeleteAsync(klantId), Times.Once);
        }

        [TestMethod]
        public async Task SetMFAStatusAsync_CallsSetMFAStatusAsyncOnRepository()
        {
            // Arrange
            var klantId = 1;
            var mfaStatus = true;

            // Act
            await _klantService.SetMFAStatusAsync(klantId, mfaStatus);

            // Assert
            _mockKlantRepository.Verify(repo => repo.SetMFAStatusAsync(klantId, mfaStatus), Times.Once);
        }

        [TestMethod]
        public async Task SetMFAFormAsync_CallsSetMFAFormAsyncOnRepository()
        {
            // Arrange
            var klantId = 1;
            var mfaForm = "test-form";

            // Act
            await _klantService.SetMFAFormAsync(klantId, mfaForm);

            // Assert
            _mockKlantRepository.Verify(repo => repo.SetMFAFormAsync(klantId, mfaForm), Times.Once);
        }


        [TestMethod]
        public async Task GetKlantWinkelmandsAsync_ReturnsWinkelmand()
        {
            // Arrange
            var winkelmandId = 1;
            var winkelmand = new Winkelmand { Id = winkelmandId, WinkelmandProducts = new List<WinkelmandProduct>() };

            _mockKlantRepository.Setup(repo => repo.GetWinkelmandsAsync(winkelmandId))
                .ReturnsAsync(winkelmand);

            // Act
            var result = await _klantService.GetKlantWinkelmandsAsync(winkelmandId);

            // Assert
            Assert.AreEqual(winkelmand, result);
        }

        [TestMethod]
        public async Task AddProductToWinkelmand_AddsProductToWinkelmand()
        {
            // Arrange
            var winkelmandId = 1;
            var klantId = 1;
            var product = new Product { Id = 1, ProductType = "Type A", ProductNaam = "Product A", ProductPrijs = 10.99, ProductKorting = 20 };
            var klant = new Klant { Id = klantId };
            var winkelmandProduct = new WinkelmandProduct { ProductId = product.Id, aantal = 1 };
            var winkelmand = new Winkelmand { Id = winkelmandId, KlantId = klantId, WinkelmandProducts = new List<WinkelmandProduct>() };

            _mockKlantRepository.Setup(repo => repo.AddProductToWinkelmand(winkelmandId, product.Id, klant))
                .ReturnsAsync(winkelmand);

            var klantService = new KlantService(_mockKlantRepository.Object);

            // Act
            var result = await klantService.AddProductToWinkelmand(winkelmandProduct, winkelmand, klant);

            // Assert
            Assert.AreEqual(1, result.WinkelmandProducts.Count);
            var addedWinkelmandProduct = result.WinkelmandProducts.First();
            Assert.AreEqual(winkelmandProduct.ProductId, addedWinkelmandProduct.ProductId);
            Assert.AreEqual(winkelmandProduct.aantal, addedWinkelmandProduct.aantal);
        }
    }
}