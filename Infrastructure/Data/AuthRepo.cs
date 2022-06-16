using Core.Dtos;
using Core.models;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _context;
        private readonly IInfrastructureConfiguration _config;

        public AuthRepo(AppDbContext context,IInfrastructureConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ActionResponse<ReturnUserDto>> CreateUser(CreateUserDto user)
        {
            
            ActionResponse<ReturnUserDto> response = new ActionResponse<ReturnUserDto>();
            if(user == null) throw new ArgumentNullException("user");
            if(user.Password != user.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Password doesnt match";
                return response;
            }
            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email Already Used";
                return response;
            }
            createPasswordHas(user.Password, out byte[] passwordHash, out byte[] passwordSlat);
            User createdUser = new User(); 
            _context.Users.Add(createdUser);
            createdUser.Email = user.Email;
            createdUser.Name = user.Name;
            createdUser.PasswordSalt = passwordSlat;
            createdUser.PasswordHash = passwordHash;
            _context.SaveChanges();

            var token = createToken(createdUser);
             
            response.Success = true;
            response.Message = "Account Created";
            ReturnUserDto returnUser = new ReturnUserDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                Name = createdUser.Name,
                Token = token,
            };

            response.Data = returnUser;
            
            return response;

        }

        public async Task<ActionResponse<ReturnUserDto>> Login(LoginDto user)
        {
            ActionResponse<ReturnUserDto> response = new ActionResponse<ReturnUserDto>();
            if (!await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "Invalid Credintial";
                return response;
            }
            var userRecord = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (!verifyPasswordHas(user.Password, userRecord.PasswordHash, userRecord.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Invalid Credintial";
                return response;
            }

            var token = createToken(userRecord);
            ReturnUserDto returnedUser = new ReturnUserDto
            {
                Name = userRecord.Name,
                Id = userRecord.Id,
                Email = userRecord.Email,
                Token = token
            };
            response.Success = true;
            response.Message = "Loged In";
            response.Data = returnedUser;

            return response;
        }

        public Task<ReturnUserDto> Logout()
        {
            throw new NotImplementedException();
        }

        private async Task<Boolean> UserExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        private void createPasswordHas(string password, out byte[] passwordHash, out byte[] passwordSlat)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSlat = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string createToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var credit = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credit
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    private bool verifyPasswordHas(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using ( var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for(int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != passwordHash[i]) return false;
            }
        }
        return true;
    }
    }

}
