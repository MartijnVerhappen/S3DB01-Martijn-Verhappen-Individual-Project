using DAL.Entities;
using DAL.Mapping;
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
            return KlantMapping.MapTo(klant);
        }

        public async Task<Klant> UpdateAsync(Klant klant)
        {
            _dbContext.Klant.Update(KlantMapping.MapTo(klant));
            await _dbContext.SaveChangesAsync();
            return klant;
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
                .FirstOrDefaultAsync(k => k.Id == id);

            return WinkelmandMapping.MapTo(klant.Winkelmand);
        }

        public async Task<Winkelmand> AddProductToWinkelmand(int winkelmandId, int productId)
        {
            var winkelmandEntity = await _dbContext.Winkelmand
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.Id == winkelmandId);

            if (winkelmandEntity != null)
            {
                var productEntity = await _dbContext.Product.FindAsync(productId);
                if (productEntity != null)
                {
                    winkelmandEntity.Products.Add(productEntity);
                    await _dbContext.SaveChangesAsync();
                    return WinkelmandMapping.MapTo(winkelmandEntity);
                }
            }

            return null;
        }
    }
}
