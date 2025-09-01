using Finanzauto.Domain.DTOS.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(AuthRequest request);
    }
}
