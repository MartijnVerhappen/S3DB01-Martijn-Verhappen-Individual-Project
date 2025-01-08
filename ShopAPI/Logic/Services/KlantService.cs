using Logic.IService;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.IRepositories;
using Logic.CustomExceptions;

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
            var klant = await _klantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                throw new DomainNotFoundException();
            }
            return klant;
        }

        public async Task<Klant> GetKlantByUsernameAsync(string username)
        {
            var klant = await _klantRepository.GetByUsernameAsync(username);
            if(klant == null)
            {
                throw new DomainNotFoundException();
            }
            return klant;
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
            try
            {
                await _klantRepository.SetMFAStatusAsync(id, mfaStatus);
            }
            catch (DomainNotFoundException)
            {
                throw;
            }
        }

        public async Task SetMFAFormAsync(int id, string mfaForm)
        {
            await _klantRepository.SetMFAFormAsync(id, mfaForm);
        }

        public async Task<Winkelmand> GetKlantWinkelmandsAsync(int klantId)
        {
            Winkelmand winkelmand = await _klantRepository.GetWinkelmandsAsync(klantId);
            if(winkelmand == null)
            {
                throw new DomainNotFoundException();
            }
            return winkelmand;
        }

        public async Task<Winkelmand> AddProductToWinkelmand(WinkelmandProduct product, Winkelmand winkelmand, Klant klant)
        {
            winkelmand.WinkelmandProducts.Add(product);
            // opslaan in database
            _klantRepository.AddProductToWinkelmand(winkelmand.Id, product.Id, klant);

            return winkelmand;
        }
    }
}
