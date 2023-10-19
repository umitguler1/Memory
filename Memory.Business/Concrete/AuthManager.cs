using AutoMapper;
using Memory.Business.Abstract;
using Memory.Entities.Concrete;
using Memory.Entities.Concrete.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Memory.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthManager(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager, SignInManager<AppIdentityUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<AppIdentityUser> GetUser(string email)
        {
            AppIdentityUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<AppIdentityUser> GetUserByUserName(string userName)
        {
            return await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        public async Task<SignInResult> Login(LoginDto loginDto)
        {
            AppIdentityUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            return user == null ? new SignInResult() : await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> AddRoleToUser(AppIdentityUser user, string role)
        {
            AppIdentityRole rol = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == role);
            if (rol is null)
            {
                await _roleManager.CreateAsync(new AppIdentityRole() { Name = role, NormalizedName = role.ToUpper() });
            }
            return await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            AppIdentityUser user = _mapper.Map<AppIdentityUser>(registerDto);
            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await AddRoleToUser(user, "User");
            }
            return result;
        }
        public async Task<string> CreateToken(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is not null)
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, user.LockoutEnabled);
                if (result.Succeeded)
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.UTF8.GetBytes("keyvaluename");
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("UserId",user.Id.ToString()),
                        new Claim("FirstName",user.FirstName),
                        new Claim("LastName",user.LastName),
                        new Claim("UserName",user.UserName),
                        new Claim("Email",user.Email)
                    };
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (string role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
                    SecurityTokenDescriptor securityToken = new SecurityTokenDescriptor
                    {
                        Subject = claimsIdentity,
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    SecurityToken jwtToken = tokenHandler.CreateToken(securityToken);
                    string token = tokenHandler.WriteToken(jwtToken);
                    return token;
                }
            }
            return null;
        }
    }
}
