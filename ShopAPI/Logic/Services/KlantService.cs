using Logic.IService;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.IRepositories;

namespace Logic.Services
{
    public class KlantService : IKlantService
    {
        private readonly IKlantRepository _klantRepository;

        public KlantService(IKlantRepository klantRepository)
        {
            _klantRepository = klantRepository;
        }

        public async Task<Klant> CreateKlantAsync(Klant klant)
        {
            return await _klantRepository.CreateAsync(klant);
        }

        public async Task<Klant> GetKlantByIdAsync(int id)
        {
            return await _klantRepository.GetByIdAsync(id);
        }

        public async Task<Klant> GetKlantByUsernameAsync(string username)
        {
            return await _klantRepository.GetByUsernameAsync(username);
        }

        public async Task<Klant> UpdateKlantAsync(Klant klant)
        {
            return await _klantRepository.UpdateAsync(klant);
        }

        public async Task DeleteKlantAsync(int id)
        {
            await _klantRepository.DeleteAsync(id);
        }

        public async Task SetMFAStatusAsync(int id, bool mfaStatus)
        {
            await _klantRepository.SetMFAStatusAsync(id, mfaStatus);
        }

        public async Task SetMFAFormAsync(int id, string mfaForm)
        {
            await _klantRepository.SetMFAFormAsync(id, mfaForm);
        }

        public async Task<Winkelmand> GetKlantWinkelmandsAsync(int winkelmandId)
        {
            return await _klantRepository.GetWinkelmandsAsync(winkelmandId);
        }

        public async Task<Winkelmand> AddProductToWinkelmand(Product product, Winkelmand winkelmand)
        {
            winkelmand.Products.Add(product);
            // opslaan in database
            _klantRepository.AddProductToWinkelmand(winkelmand.Id, product.Id);

            return winkelmand;
        }
    }
}
