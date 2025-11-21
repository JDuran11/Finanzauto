using Finanzauto.Application.Interfaces;
using Finanzauto.Domain.DTOS.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

        [EnableRateLimiting("auth")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.AuthenticateAsync(request);
            if (response == null)
                return Unauthorized(new { Message = "Usuario o contraseña incorrectos" });

            return Ok(response);
        }
    }


}
