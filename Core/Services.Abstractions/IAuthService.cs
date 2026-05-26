using Shared;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        // Check Email Exists
        Task<bool> CheckEmailExistsAsync(string email);
        // Get Current User
        Task<UserResultDto> GetCurrentUserAsync(string email);

        // Get Current User Address
        Task<AddressDto> GetCurrentUserAddressAsync(string email);
        // Update Current User Address
        Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto,string email);
    }
}
