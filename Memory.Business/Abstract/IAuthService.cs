using Memory.Entities.Concrete;
using Memory.Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Memory.Business.Abstract
{
    public interface IAuthService
    {
        Task<IdentityResult> AddRoleToUser(AppIdentityUser user, string role);
        Task<SignInResult> Login(LoginDto loginDto);
        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<AppIdentityUser> GetUser(string email);
        Task<AppIdentityUser> GetUserByUserName(string userName);
        Task<string> CreateToken(LoginDto loginDto);
        Task Logout();
    }
}
