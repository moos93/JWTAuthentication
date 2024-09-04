using Application.Contracts;
using Application.DTO;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repo
{
    internal class UserRepo : IUser
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;

        public UserRepo(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }
        public async Task<LoginRespons> LoginUserAsync(LogInDTO loginDTO)
        {
            var getUser = await FindUserByEmail(loginDTO.Email);
            if (getUser == null)
                return new LoginRespons(false, "User not found");
            bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (checkPassword)
                return new LoginRespons(true, "Login Successfull", GenerateJWTToken(getUser));
            else return new LoginRespons(false, "Invalid ");

        }

        private string GenerateJWTToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials= new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name!),
            new Claim(ClaimTypes.Email, user.Email!),
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ApplicationUser> FindUserByEmail(string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<RegistrationRespons> RegisterUserAsync(RegisterUserDTO RegisterUser)
        {
            var getUser = await FindUserByEmail(RegisterUser.Email!);
            if (getUser != null)
                return new RegistrationRespons(false, "User already exist");
            appDbContext.Users.Add(new ApplicationUser()
            {
                Name = RegisterUser.Name,
                Email = RegisterUser.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(RegisterUser.Password)
            }
                );
            await appDbContext.SaveChangesAsync();
            return new RegistrationRespons(true, "Registaration completd");
        }

    }
}
