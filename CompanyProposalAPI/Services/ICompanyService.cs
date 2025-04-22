using CompanyProposalAPI.Models.DataModels;

namespace CompanyProposalAPI.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company> CreateCompanyAsync(Company company);
        Task UpdateCompanyAsync(int id, Company company);
        Task DeleteCompanyAsync(int id);
        Task<IEnumerable<Item>> GetCompanyItemsAsync(int companyId);
        Task AddItemToCompanyAsync(int companyId, int itemId);
        Task RemoveItemFromCompanyAsync(int companyId, int itemId);
    }
} 