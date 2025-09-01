using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Finanzauto.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.AuthenticateAsync(request);
            if (response == null)
                return Unauthorized();

            return Ok(response);
        }
    }


}
