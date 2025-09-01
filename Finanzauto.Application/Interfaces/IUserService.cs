using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(string username, string plainPassword, string email);
    }
}
