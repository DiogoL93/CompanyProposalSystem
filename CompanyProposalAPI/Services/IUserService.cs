using CompanyProposalAPI.Models.DataModels;

namespace CompanyProposalAPI.Services
{
    public interface IUserService
    {
        Task<User?> GetCurrentUserAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
    }
} 