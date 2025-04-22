using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CompanyProposalAPI.Models.DataModels;
using System.Collections.Generic;

namespace CompanyProposalAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();

            // Apply any pending migrations
            await context.Database.MigrateAsync();

            // Check if we already have data
            if (await context.Companies.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Create companies
            var companies = new[]
            {
                new Company { 
                    Name = "Tech Solutions Inc.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Users = new List<User>(),
                    Items = new List<Item>()
                },
                new Company { 
                    Name = "Global Industries",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Users = new List<User>(),
                    Items = new List<Item>()
                },
                new Company { 
                    Name = "Service Providers Ltd",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Users = new List<User>(),
                    Items = new List<Item>()
                },
                new Company { 
                    Name = "Retail Corp",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Users = new List<User>(),
                    Items = new List<Item>()
                }
            };
            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();

            // Create users
            var users = new[]
            {
                new User { 
                    Username = "admin1", 
                    Email = "admin1@techsolutions.com", 
                    PasswordHash = "hashed_password1", 
                    FirstName = "John", 
                    LastName = "Doe", 
                    CompanyId = companies[0].Id,
                    Company = companies[0],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User { 
                    Username = "admin2", 
                    Email = "admin2@globalindustries.com", 
                    PasswordHash = "hashed_password2", 
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    CompanyId = companies[1].Id,
                    Company = companies[1],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User { 
                    Username = "admin3", 
                    Email = "admin3@serviceproviders.com", 
                    PasswordHash = "hashed_password3", 
                    FirstName = "Bob", 
                    LastName = "Johnson", 
                    CompanyId = companies[2].Id,
                    Company = companies[2],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User { 
                    Username = "admin4", 
                    Email = "admin4@retailcorp.com", 
                    PasswordHash = "hashed_password4", 
                    FirstName = "Alice", 
                    LastName = "Williams", 
                    CompanyId = companies[3].Id,
                    Company = companies[3],
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Create items
            var items = new[]
            {
                new Item { 
                    Name = "Enterprise Software License",
                    Price = 50000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Companies = new List<Company>(),
                    Proposals = new List<Proposal>()
                },
                new Item { 
                    Name = "Hardware Maintenance",
                    Price = 25000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Companies = new List<Company>(),
                    Proposals = new List<Proposal>()
                },
                new Item { 
                    Name = "Cloud Storage",
                    Price = 1200.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Companies = new List<Company>(),
                    Proposals = new List<Proposal>()
                },
                new Item { 
                    Name = "Consulting Services",
                    Price = 15000.00m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Companies = new List<Company>(),
                    Proposals = new List<Proposal>()
                }
            };
            await context.Items.AddRangeAsync(items);
            await context.SaveChangesAsync();

            // Associate items with companies
            companies[0].Items.Add(items[0]);
            companies[1].Items.Add(items[1]);
            companies[2].Items.Add(items[2]);
            companies[3].Items.Add(items[3]);
            await context.SaveChangesAsync();

            // Create proposals
            var proposals = new[]
            {
                new Proposal
                {
                    Description = "Enterprise software proposal",
                    Status = ProposalStatus.Pending,
                    CreatedByUserId = users[0].Id,
                    UpdatedByUserId = users[0].Id,
                    ItemId = items[0].Id,
                    TotalValue = 45000.00m,
                    ValueType = ProposalValueType.Amount,
                    Currency = Currency.USD,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedByUser = users[0],
                    UpdatedByUser = users[0],
                    Item = items[0]
                },
                new Proposal
                {
                    Description = "Hardware maintenance proposal",
                    Status = ProposalStatus.Pending,
                    CreatedByUserId = users[1].Id,
                    UpdatedByUserId = users[1].Id,
                    ItemId = items[1].Id,
                    TotalValue = 22500.00m,
                    ValueType = ProposalValueType.Amount,
                    Currency = Currency.USD,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedByUser = users[1],
                    UpdatedByUser = users[1],
                    Item = items[1]
                },
                new Proposal
                {
                    Description = "Cloud storage proposal",
                    Status = ProposalStatus.Pending,
                    CreatedByUserId = users[2].Id,
                    UpdatedByUserId = users[2].Id,
                    ItemId = items[2].Id,
                    TotalValue = 1000.00m,
                    ValueType = ProposalValueType.Amount,
                    Currency = Currency.USD,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedByUser = users[2],
                    UpdatedByUser = users[2],
                    Item = items[2]
                }
            };
            await context.Proposals.AddRangeAsync(proposals);
            await context.SaveChangesAsync();
        }
    }
} 