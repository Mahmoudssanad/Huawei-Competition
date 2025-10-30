using GreenEye.Application.DTOs;
using GreenEye.Application.IRepository;
using GreenEye.Application.Responses;
using GreenEye.Domain.Entities;
using GreenEye.Infrasturcture.Conversion;
using GreenEye.Infrasturcture.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GreenEye.Infrasturcture.Repository
{
    public class AccountRepository(UserManager<ApplicationUser> _userManager, 
        RoleManager<IdentityRole> _roleManager, IConfiguration configuration) : IAccountRepository
    {
        public async Task<Response> RegisterAsync(RegisterDTO registerDTO)
        {
            var existingEmail = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingEmail is not null)
                return new Response(false, "Can not registered by this email");

            var getUser = UserConversion.ToApplicationUser(registerDTO);

            var createUser = await _userManager.CreateAsync(getUser, registerDTO.Password);

            if(createUser.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync(registerDTO.Role.ToString()))
                {
                    await _userManager.AddToRoleAsync(getUser, registerDTO.Role.ToString());
                }
            }

            return createUser.Succeeded ? new Response(true, "User registered successfully") 
                : new Response(false, "Error occurred while registered");
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            // get user and verify email
            var getUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return new Response(false, "Invalid Email or Password");

            // password verify
            var verifyPassword = await _userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!verifyPassword)
                return new Response(false, "Invalid Email or Password");

            // generate token
            string token = await GenerateToken(getUser);

            return new Response(true, token);
        }

        // Generate Token 
        private async Task<string> GenerateToken(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, applicationUser.Id),
                new(ClaimTypes.Name, applicationUser.UserName!),
                new(ClaimTypes.Email, applicationUser.Email!),
            };
            // get role
            var userRoles = await _userManager.GetRolesAsync(applicationUser);
            foreach(var role in userRoles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var key = Encoding.UTF8.GetBytes(configuration["JWTAuthentication:Key"]!);

            var securitKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(securitKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: configuration["JWTAuthentication:Issuer"],
                audience: configuration["JWTAuthentication:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
