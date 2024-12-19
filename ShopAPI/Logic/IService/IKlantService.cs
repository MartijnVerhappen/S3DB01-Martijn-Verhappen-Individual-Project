using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IService
{
    public interface IKlantService
    {
        Task<Klant> GetKlantByIdAsync(int id);

        Task<Klant> CreateKlantAsync(Klant klant);

        Task<Klant> GetKlantByUsernameAsync(string username);

        Task<Klant> UpdateKlantAsync(Klant klant);

        Task DeleteKlantAsync(int id);

        Task SetMFAStatusAsync(int id, bool mfaStatus);

        Task SetMFAFormAsync(int id, string mfaForm);

        Task<Winkelmand> GetKlantWinkelmandsAsync(int id);
        Task<Winkelmand> AddProductToWinkelmand(WinkelmandProduct product, Winkelmand winkelmand, Klant klant);
    }
}
