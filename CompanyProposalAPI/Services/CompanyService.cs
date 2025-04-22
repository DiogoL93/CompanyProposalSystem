using Microsoft.EntityFrameworkCore;
using CompanyProposalAPI.Models.DataModels;
using CompanyProposalAPI.Data;

namespace CompanyProposalAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .Include(c => c.Users)
                .Include(c => c.Items)
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.Users)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            company.CreatedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task UpdateCompanyAsync(int id, Company company)
        {
            var existingCompany = await _context.Companies.FindAsync(id);
            if (existingCompany == null)
            {
                throw new KeyNotFoundException($"Company with ID {id} not found.");
            }

            existingCompany.Name = company.Name;
            existingCompany.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Users)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {id} not found.");
            }

            if (company.Users.Any())
            {
                throw new InvalidOperationException("Cannot delete a company that has users.");
            }

            if (company.Items.Any())
            {
                throw new InvalidOperationException("Cannot delete a company that has items.");
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetCompanyItemsAsync(int companyId)
        {
            var company = await _context.Companies
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {companyId} not found.");
            }

            return company.Items;
        }

        public async Task AddItemToCompanyAsync(int companyId, int itemId)
        {
            var company = await _context.Companies
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {companyId} not found.");
            }

            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found.");
            }

            if (company.Items.Any(i => i.Id == itemId))
            {
                throw new InvalidOperationException("Item is already associated with this company.");
            }

            company.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCompanyAsync(int companyId, int itemId)
        {
            var company = await _context.Companies
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {companyId} not found.");
            }

            var item = company.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} is not associated with this company.");
            }

            company.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
} 