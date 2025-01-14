using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IRepositories
{
    public interface IUserRepository
    {
        Task<Klant> ValidateUserAsync(string username, string password);
        bool VerifyPassword(string password, string storedHash);
    }
}
