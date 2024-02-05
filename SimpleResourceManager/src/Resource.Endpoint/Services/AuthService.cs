using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Resource.Domain.Entity;
using Resource.Endpoint.Abstract;
using Resource.Endpoint.Options;
using Resource.Messages.Models;

namespace Resource.Endpoint.Services;

public class AuthService: IAuthService
{
        private readonly UserManager<ReservationHolder> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<JwtOptions> _jwtOptions;
        
        public AuthService(UserManager<ReservationHolder> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions;
        }
        public async Task<(int,string)> Registeration(RegistrationModel model,string role)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");

            ReservationHolder user = new ReservationHolder()
            {
                Email = model.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };
            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return (0,"User creation failed! Please check user details and try again.");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);

            return (1,"User created successfully!");
        }

        public async Task<(int,string)> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (0, "Invalid username");
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Invalid password");
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
              authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return (1, token);
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
}