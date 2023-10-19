using Memory.Business.Abstract;
using Memory.Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           string token = await _authService.CreateToken(loginDto);
           return token is not null ? Ok(token) : BadRequest();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
           var result = await  _authService.Register(registerDto);
           return result.Succeeded ? Ok("Kayıt işlemi başarılı.") : BadRequest(result.Errors);
        }
    }
}
