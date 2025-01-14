using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IService
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(string username, string password);
    }
}
