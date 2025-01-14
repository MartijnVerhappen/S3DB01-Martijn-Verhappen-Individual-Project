using DAL.Mapping;
using Logic.IRepositories;
using Logic.Models;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;

        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Klant> ValidateUserAsync(string username, string password)
        {
            var user = _dbContext.Klant.FirstOrDefault(u => u.Gebruikersnaam == username);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(password, user.WachtwoordHash))
            {
                return null;
            }

            var mappedUser = KlantMapping.MapTo(user);
            return mappedUser;
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
