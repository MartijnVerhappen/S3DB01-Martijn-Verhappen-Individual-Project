using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IRepositories
{
    public interface IKlantRepository
    {
        Task<Klant> CreateAsync(Klant klant);

        Task<Klant> GetByIdAsync(int id);

        Task<Klant> GetByUsernameAsync(string username);

        Task<Klant> UpdateAsync(Klant klant);

        Task DeleteAsync(int id);

        Task SetMFAStatusAsync(int id, bool mfaStatus);

        Task SetMFAFormAsync(int id, string mfaForm);

        Task<Winkelmand> GetWinkelmandsAsync(int id);

        Task<Winkelmand> AddProductToWinkelmand(int winkelmandId, int productId);
    }

}
