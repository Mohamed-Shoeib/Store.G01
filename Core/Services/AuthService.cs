using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
using Shared.ErrorsModel;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager, IOptions<JwtOptions> jwtOptions,IMapper mapper) : IAuthService
    {
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include( u=> u.Address).FirstOrDefaultAsync(u => u.Email == email);
            if(user is null)
            {
                throw new UserNotFoundException(email);
            }
            var result = mapper.Map<AddressDto>(user.Address);
            return result;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null)
            {
                throw new UserNotFoundException(email);
            }
            return new UserResultDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await GenerateJwtToken(user)
            };
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }
            if (user.Address is not null)
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
            }
            else
            {
                 var addressResult = mapper.Map<Address>(addressDto);
                 user.Address = addressResult;
            }
            await userManager.UpdateAsync(user);
            return addressDto;
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new UnauthorizedException();
            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnauthorizedException();
            return new UserResultDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await GenerateJwtToken(user)
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto RegisterDto)
        {
            // Vaildate Email is Exist or Not

            if (await CheckEmailExistsAsync(RegisterDto.Email))
            {
                throw new DuplicateUserEmailBadRequestException(RegisterDto.Email);
            }

            var user = new AppUser
            {
                UserName = RegisterDto.UserName,
                DisplayName = RegisterDto.DisplayName,
                Email = RegisterDto.Email,
                PhoneNumber = RegisterDto.PhoneNumber.ToString()
            };
            var result = await userManager.CreateAsync(user, RegisterDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new VaildationException(errors);
            }
                return new UserResultDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await GenerateJwtToken(user)
                };
        }
        
        private async Task<string> GenerateJwtToken(AppUser user)
        {
            // Header
            // Payload
            // Signature

            var JwtOptions = jwtOptions.Value;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var secertKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(JwtOptions.ExpireTime),
                signingCredentials: new SigningCredentials(secertKey,SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
