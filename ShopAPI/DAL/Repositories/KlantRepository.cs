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
            _dbContext.Klant.Add(klant);
            await _dbContext.SaveChangesAsync();
            return klant;
        }

        public async Task<Klant> GetByIdAsync(int id)
        {
            return await _dbContext.Klant
                .Include(k => k.Winkelmand)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Klant> GetByUsernameAsync(string username)
        {
            return await _dbContext.Klant
                .Include(k => k.Winkelmand)
                .FirstOrDefaultAsync(k => k.Gebruikersnaam == username);
        }

        public async Task<Klant> UpdateAsync(Klant klant)
        {
            _dbContext.Klant.Update(klant);
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
            var klant = await _dbContext.Klant
                .Include(k => k.Winkelmand)
                .FirstOrDefaultAsync(k => k.Id == id);
            return klant?.Winkelmand;
        }
    }
}
