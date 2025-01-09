using DAL;
using DAL.Entities;
using DAL.Repositories;
using Logic.CustomExceptions;
using Logic.IService;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopAPI.Controllers;
using ShopAPI.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_tests.ControllerTests
{
    [TestClass]
    public class KlantControllerIntegrationtest
    {
        private readonly Mock<IKlantService> _mockService;
        private readonly DbContextOptions<DBContext> _options;
        public KlantControllerIntegrationtest()
        {
            _options = new DbContextOptionsBuilder<DBContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            using (DBContext context = new DBContext(_options))
            {
                context.Database.EnsureCreated();
            }

            _mockService = new Mock<IKlantService>();
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
        public async Task Test_Controller_CreateKlant_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var newKlant = new Klant
                {
                    Gebruikersnaam = "newuser",
                    WachtwoordHash = "newpassword",
                    MFAStatus = false,
                    MFAVorm = "authenticator_app"
                };

                var result = await controller.CreateKlant(newKlant);

                // Assert the result is a CreatedAtActionResult
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

                // Extract the created klant
                var createdKlant = (result as CreatedAtActionResult)?.Value as Klant;

                Assert.IsNotNull(createdKlant);
                Assert.AreEqual("newuser", createdKlant.Gebruikersnaam);
                Assert.AreEqual("newpassword", createdKlant.WachtwoordHash);
                Assert.IsFalse(createdKlant.MFAStatus);
                Assert.AreEqual("authenticator_app", createdKlant.MFAVorm);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetKlantById_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var result = await controller.GetKlantById(1);

                Assert.IsNotNull(result.Value);
                Assert.AreEqual("testuser1", result.Value.Gebruikersnaam);
                Assert.AreEqual("SuperEchtWachtwoord1", result.Value.WachtwoordHash);
                Assert.IsTrue(result.Value.MFAStatus);
                Assert.IsNotNull(result.Value.MFAVorm);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetKlantByUsername_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var result = await controller.GetKlantByUsername("testuser1");

                Assert.IsNotNull(result.Value);
                Assert.AreEqual("testuser1", result.Value.Gebruikersnaam);
                Assert.IsTrue(result.Value.MFAStatus);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetKlantByUsername_NotFound()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.GetKlantByUsername("nonexistentuser");
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }

        [TestMethod]
        public async Task Test_Controller_UpdateKlant_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var updatedKlant = new Klant
                {
                    Id = 1,
                    Gebruikersnaam = "updateduser",
                    WachtwoordHash = "updatedpassword",
                    MFAStatus = false,
                    MFAVorm = "SMS"
                };

                var result = await controller.UpdateKlant(1, updatedKlant);

                Assert.IsInstanceOfType(result, typeof(NoContentResult));
                var klant = await context.Klant.FindAsync(1);
                Assert.AreEqual("updateduser", klant.Gebruikersnaam);
            }
        }

        [TestMethod]
        public async Task Test_Controller_UpdateKlant_BadRequest()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var updatedKlant = new Klant
                {
                    Id = 2,
                    Gebruikersnaam = "updateduser",
                    WachtwoordHash = "updatedpassword",
                    MFAStatus = false,
                    MFAVorm = "SMS"
                };

                var result = await controller.UpdateKlant(1, updatedKlant);

                Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            }
        }

        [TestMethod]
        public async Task Test_Controller_DeleteKlant_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var result = await controller.DeleteKlant(1);

                Assert.IsInstanceOfType(result, typeof(NoContentResult));
                var klant = await context.Klant.FindAsync(1);
                Assert.IsNull(klant);
            }
        }

        [TestMethod]
        public async Task Test_Controller_DeleteKlant_NotFound()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.DeleteKlant(999);
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }

        [TestMethod]
        public async Task Test_Controller_SetMFAStatus_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var result = await controller.SetMFAStatus(1, true);

                Assert.IsInstanceOfType(result, typeof(NoContentResult));
                var klant = await context.Klant.FindAsync(1);
                Assert.IsTrue(klant.MFAStatus);
            }
        }

        [TestMethod]
        public async Task Test_Controller_SetMFAStatus_NotFound()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);               

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.SetMFAStatus(999, true);
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetKlantWinkelmand_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var result = await controller.GetKlantWinkelmand(1);

                Assert.IsNotNull(result.Value);
                Assert.AreEqual(2, result.Value.WinkelmandProducts.Count);
            }
        }

        [TestMethod]
        public async Task Test_Controller_GetKlantWinkelmand_NotFound()
        {
            using (var context = new DBContext(_options))
            {
                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.GetKlantWinkelmand(999);
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }

        [TestMethod]
        public async Task Test_Controller_AddProductToWinkelmand_HappyFlow()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                // Create the ProductRequest with product and quantity
                var productRequest = new ProductRequest
                {
                    product = new Product
                    {
                        Id = 1, // Assume product with ID 1 exists
                        ProductType = "Electronics",
                        ProductNaam = "Smartphone",
                        ProductPrijs = 299.99,
                        ProductKorting = 10
                    },
                    aantal = 2
                };

                var result = await controller.AddProductToWinkelmand(productRequest, 1, 1); // Assume winkelmandId and klantId are 1

                var okResult = result as OkObjectResult;

                Assert.IsNotNull(okResult);
                Assert.AreEqual(200, okResult.StatusCode);

                var updatedWinkelmand = okResult.Value as Winkelmand;
                Assert.IsNotNull(updatedWinkelmand);
                Assert.IsTrue(updatedWinkelmand.WinkelmandProducts.Any(p => p.ProductId == 1 && p.aantal == 2));
            }
        }

        [TestMethod]
        public async Task Test_Controller_AddProductToWinkelmand_WinkelmandNotFound()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var productRequest = new ProductRequest
                {
                    product = new Product
                    {
                        Id = 1,
                        ProductType = "Electronics",
                        ProductNaam = "Smartphone",
                        ProductPrijs = 299.99,
                        ProductKorting = 10
                    },
                    aantal = 2
                };

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.AddProductToWinkelmand(productRequest, 1, 999);
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }


        [TestMethod]
        public async Task Test_Controller_AddProductToWinkelmand_KlantNotFound()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var productRequest = new ProductRequest
                {
                    product = new Product
                    {
                        Id = 1,
                        ProductType = "Electronics",
                        ProductNaam = "Smartphone",
                        ProductPrijs = 299.99,
                        ProductKorting = 10
                    },
                    aantal = 2
                };

                var exception = await Assert.ThrowsExceptionAsync<DomainNotFoundException>(async () =>
                {
                    await controller.AddProductToWinkelmand(productRequest, 1, 999);
                });

                Assert.AreEqual("Failed to get Entity", exception.Message);
            }
        }


        [TestMethod]
        public async Task Test_Controller_AddProductToWinkelmand_InvalidInput()
        {
            using (var context = new DBContext(_options))
            {
                await SeedDatabase(context);

                var service = new KlantService(new KlantRepository(context));
                var controller = new KlantController(service);

                var productRequest = new ProductRequest
                {
                    product = new Product
                    {
                        Id = 0,
                        ProductType = "Unknown",
                        ProductNaam = "Invalid Product",
                        ProductPrijs = 0,
                        ProductKorting = 0
                    },
                    aantal = -1
                };

                var result = await controller.AddProductToWinkelmand(productRequest, 1, 1);

                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            }
        }
    }
}
