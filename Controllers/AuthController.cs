using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTaurang.Services.IServices;
using static RESTaurang.Dtos.AuthDtos;

namespace RESTaurang.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var token = await _auth.LoginAsync(dto.Username, dto.Password);
            if (token is null) return Unauthorized();

            return Ok(new LoginResponseDto(token));
        }
    }
}
