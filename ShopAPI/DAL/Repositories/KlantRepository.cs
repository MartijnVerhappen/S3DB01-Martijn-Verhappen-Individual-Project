using DAL.Entities;
using DAL.Mapping;
using Logic.CustomExceptions;
using Logic.IRepositories;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        private readonly DBContext _dbContext;

        public KlantRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Klant> CreateAsync(Klant klant)
        {
            _dbContext.Klant.Add(KlantMapping.MapTo(klant));
            await _dbContext.SaveChangesAsync();
            return klant;
        }

        public async Task<Klant> GetByIdAsync(int id)
        {
            KlantEntity klant = await _dbContext.Klant
                .Include(k => k.Winkelmand)
                .FirstOrDefaultAsync(k => k.Id == id);
            if (klant == null)
            {
                return null;
            }
            return KlantMapping.MapTo(klant);
        }

        public async Task<Klant> GetByUsernameAsync(string username)
        {
            KlantEntity klant = await _dbContext.Klant
                .Include(k => k.Winkelmand)
                .FirstOrDefaultAsync(k => k.Gebruikersnaam == username);
            if (klant == null)
            {
                return null;
            }
            return KlantMapping.MapTo(klant);
        }

        public async Task<Klant> UpdateAsync(Klant klant)
        {
            var existingEntity = await _dbContext.Klant.FindAsync(klant.Id);
            if (existingEntity != null)
            {
                // Update properties
                existingEntity.Gebruikersnaam = klant.Gebruikersnaam;
                existingEntity.MFAStatus = klant.MFAStatus;
                existingEntity.MFAVorm = klant.MFAVorm;

                await _dbContext.SaveChangesAsync();
                return KlantMapping.MapTo(existingEntity);
            }

            throw new KeyNotFoundException("Klant not found");
        }

        public async Task DeleteAsync(int id)
        {
            var klant = await _dbContext.Klant.FindAsync(id);
            _dbContext.Klant.Remove(klant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetMFAStatusAsync(int id, bool mfaStatus)
        {
            var klant = await _dbContext.Klant.FindAsync(id);
            if (klant == null)
            {
                throw new DomainNotFoundException();
            }

            klant.MFAStatus = mfaStatus;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetMFAFormAsync(int id, string mfaForm)
        {
            var klant = await _dbContext.Klant.FindAsync(id);
            klant.MFAVorm = mfaForm;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Winkelmand> GetWinkelmandsAsync(int id)
        {
            KlantEntity klant = await _dbContext.Klant
            .Include(k => k.Winkelmand)
                .ThenInclude(w => w.WinkelmandProducts)
                    .ThenInclude(wp => wp.Product)
            .FirstOrDefaultAsync(k => k.Id == id);

            if (klant == null)
            {
                return null;
            }

            Klant klantModel = KlantMapping.MapTo(klant);

            return WinkelmandMapping.MapTo(klant.Winkelmand, klantModel);
        }

        public async Task<Winkelmand> AddProductToWinkelmand(int winkelmandId, int productId, Klant klant)
        {
            var winkelmandEntity = await _dbContext.Winkelmand
                .Include(w => w.WinkelmandProducts)
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Id == winkelmandId && w.KlantId == klant.Id);

            if (winkelmandEntity != null)
            {
                var productEntity = await _dbContext.Product.FindAsync(productId);

                if (productEntity != null)
                {
                    var existingWinkelmandProduct = winkelmandEntity.WinkelmandProducts.FirstOrDefault(wp => wp.ProductId == productId);

                    if (existingWinkelmandProduct == null)
                    {
                        var newWinkelmandProduct = new WinkelmandProductEntity
                        {
                            WinkelmandId = winkelmandEntity.Id,
                            Winkelmand = winkelmandEntity,
                            ProductId = productEntity.Id,
                            Product = productEntity,
                            aantal = 1
                        };
                        winkelmandEntity.WinkelmandProducts.Add(newWinkelmandProduct);
                    }
                    else
                    {
                        existingWinkelmandProduct.aantal++;
                    }

                    await _dbContext.SaveChangesAsync();

                    var winkelmandModel = WinkelmandMapping.MapTo(winkelmandEntity, klant);
                    return winkelmandModel;
                }
            }

            return null;
        }

    }
}
