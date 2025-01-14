﻿using DAL;
using DAL.Entities;
using DAL.Repositories;
using Logic.IService;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopAPI.Controllers;

namespace Unit_tests.RepositoryTests
{

    [TestClass]
    public class KlantRepositoryIntegrationTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly DbContextOptions<DBContext> _options;

        public KlantRepositoryIntegrationTests()
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
            var product1 = new ProductEntity
            {
                Id = 1,
                ProductType = "Type A",
                ProductNaam = "Product A",
                ProductPrijs = 10.99,
                ProductKorting = 20
            };

            var product2 = new ProductEntity
            {
                Id = 2,
                ProductType = "Type B",
                ProductNaam = "Product B",
                ProductPrijs = 15.99,
                ProductKorting = 10
            };

            var product3 = new ProductEntity
            {
                Id = 3,
                ProductType = "Type C",
                ProductNaam = "Product C",
                ProductPrijs = 19.99,
                ProductKorting = 0
            };

            if (!await context.Product.AnyAsync())
            {
                await context.Product.AddRangeAsync(product1, product2, product3);
                await context.SaveChangesAsync();
            }

            var winkelmand1 = await context.Winkelmand.FirstOrDefaultAsync(w => w.Id == 1);
            if (winkelmand1 == null)
            {
                winkelmand1 = new WinkelmandEntity
                {
                    Id = 1,
                    KlantId = 1,
                    AanmaakDatum = DateTime.Now,
                    LaatsteVeranderingsDatum = DateTime.Now,
                    WinkelmandProducts = new List<WinkelmandProductEntity>
            {
                new WinkelmandProductEntity
                {
                    ProductId = product1.Id,
                    Product = product1,
                    aantal = 2
                },
                new WinkelmandProductEntity
                {
                    ProductId = product2.Id,
                    Product = product2,
                    aantal = 1
                }
            }
                };
                await context.Winkelmand.AddAsync(winkelmand1);
            }

            var winkelmand2 = await context.Winkelmand.FirstOrDefaultAsync(w => w.Id == 2);
            if (winkelmand2 == null)
            {
                winkelmand2 = new WinkelmandEntity
                {
                    Id = 2,
                    KlantId = 2,
                    AanmaakDatum = DateTime.Now,
                    LaatsteVeranderingsDatum = DateTime.Now,
                    WinkelmandProducts = new List<WinkelmandProductEntity>
            {
                new WinkelmandProductEntity
                {
                    ProductId = product1.Id,
                    Product = product1,
                    aantal = 1
                }
            }
                };
                await context.Winkelmand.AddAsync(winkelmand2);
            }

            await context.SaveChangesAsync();

            if (!await context.Klant.AnyAsync())
            {
                var klant1 = new KlantEntity
                {
                    Id = 1,
                    Gebruikersnaam = "testuser1",
                    WachtwoordHash = "SuperEchtWachtwoord1",
                    MFAStatus = true,
                    MFAVorm = "Authenticator_app",
                    Winkelmand = winkelmand1
                };

                var klant2 = new KlantEntity
                {
                    Id = 2,
                    Gebruikersnaam = "testuser2",
                    WachtwoordHash = "SuperEchtWachtwoord2",
                    MFAStatus = false,
                    MFAVorm = "SMS",
                    Winkelmand = winkelmand2
                };

                await context.Klant.AddRangeAsync(klant1, klant2);
                await context.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_CreatesNewKlant()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var newKlant = new Klant { Id = 3, Gebruikersnaam = "newuser", WachtwoordHash = "NewPassword123", MFAStatus = false, MFAVorm = "SMS" };

                // Act
                var result = await repository.CreateAsync(newKlant);

                // Assert
                Assert.AreEqual(newKlant, result);
                Assert.AreEqual(3, result.Id);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsKlant()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klantId = 1;

                // Act
                var result = await repository.GetByIdAsync(klantId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("testuser1", result.Gebruikersnaam);
                Assert.AreEqual(true, result.MFAStatus);
                Assert.AreEqual("Authenticator_app", result.MFAVorm);
            }
        }

        [TestMethod]
        public async Task GetByUsernameAsync_ReturnsKlant()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var username = "testuser2";

                // Act
                var result = await repository.GetByUsernameAsync(username);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("testuser2", result.Gebruikersnaam);
                Assert.AreEqual(false, result.MFAStatus);
                Assert.AreEqual("SMS", result.MFAVorm);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesKlant()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klant = await repository.GetByIdAsync(1);

                // Act
                klant.Gebruikersnaam = "updateduser";
                klant.MFAStatus = false;
                klant.MFAVorm = "SMS";
                var updatedKlant = await repository.UpdateAsync(klant);

                // Assert
                Assert.AreEqual("updateduser", updatedKlant.Gebruikersnaam);
                Assert.IsFalse(updatedKlant.MFAStatus);
                Assert.AreEqual("SMS", updatedKlant.MFAVorm);
            }
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesKlant()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klant = await repository.GetByIdAsync(1);

                // Act
                await repository.DeleteAsync(klant.Id);

                // Assert
                var deletedKlant = await repository.GetByIdAsync(klant.Id);
                Assert.IsNull(deletedKlant);
            }
        }

        [TestMethod]
        public async Task SetMFAStatusAsync_UpdatesMFAStatus()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klant = await repository.GetByIdAsync(1);

                // Act
                await repository.SetMFAStatusAsync(klant.Id, false);

                // Assert
                var updatedKlant = await repository.GetByIdAsync(klant.Id);
                Assert.IsFalse(updatedKlant.MFAStatus);
            }
        }

        [TestMethod]
        public async Task SetMFAFormAsync_UpdatesMFAForm()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klant = await repository.GetByIdAsync(1);

                // Act
                await repository.SetMFAFormAsync(klant.Id, "SMS");

                // Assert
                var updatedKlant = await repository.GetByIdAsync(klant.Id);
                Assert.AreEqual("SMS", updatedKlant.MFAVorm);
            }
        }

        [TestMethod]
        public async Task GetWinkelmandsAsync_ReturnsWinkelmand()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                var klantId = 1;

                // Act
                var result = await repository.GetWinkelmandsAsync(klantId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.WinkelmandProducts.Count);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.KlantId);
            }
        }

        [TestMethod]
        public async Task AddProductToWinkelmand_AddsProductToWinkelmand()
        {
            // Arrange
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);
                KlantRepository repository = new KlantRepository(context);
                int winkelmandId = 2;
                int productId = 3;
                int klantId = 2;
                Klant klant = await repository.GetByIdAsync(klantId);

                // Act
                Winkelmand result = await repository.AddProductToWinkelmand(winkelmandId, productId, klant);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.WinkelmandProducts.Count);
                Assert.AreEqual(winkelmandId, result.Id);
                Assert.AreEqual(klant.Id, result.KlantId);
                Assert.IsTrue(result.WinkelmandProducts.Any(p => p.ProductId == productId));
            }
        }
    }
}
