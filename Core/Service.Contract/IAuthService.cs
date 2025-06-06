using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Service.Contract
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}
