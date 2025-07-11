using Core.Entities;
using Core.Entities.Ignored;
using Core.Specifications;

namespace Core.Service.Contract
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser user);
        Task<bool> SendEmailAsync(Email email, MessageType type = MessageType.ResetPassword);

        /// <summary>
        /// Check if email exists and valid or not.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>returns null if email expired or doesn't exist. Otherwise returns email</returns>
        Task<Email> CheckEmailAsync(string code);
        //Task<IEnumerable<AppUser>> GetAllAsync(PaginationQueryParams query, bool tracking = false);
    }
}
