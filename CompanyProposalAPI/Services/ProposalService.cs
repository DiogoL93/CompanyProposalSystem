using Microsoft.EntityFrameworkCore;
using CompanyProposalAPI.Data;
using CompanyProposalAPI.Models.DataModels;
using CompanyProposalAPI.Models.Requests;

namespace CompanyProposalAPI.Services
{
    public class ProposalService : IProposalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ProposalService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<Proposal>> GetProposalsByItemAsync(int itemId)
        {
            return await _context.Proposals
                .Include(p => p.CreatedByUser)
                .Include(p => p.UpdatedByUser)
                .Include(p => p.Item)
                .Where(p => p.ItemId == itemId)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Proposal> CreateProposalWithItemsAsync(ProposalRequest request)
        {
            // Check if there's already a pending proposal for this item
            var existingPendingProposal = await _context.Proposals
                .FirstOrDefaultAsync(p => p.ItemId == request.ItemId && (p.Status == ProposalStatus.Pending || p.Status == ProposalStatus.Accepted));

            if (existingPendingProposal != null)
            {
                throw new InvalidOperationException($"A pending or accepted proposal already exists for item with ID {request.ItemId}");
            }

            var proposal = new Proposal
            {
                Description = request.Description,
                ItemId = request.ItemId,
                TotalValue = request.TotalValue,
                ValueType = request.ValueType,
                Currency = request.Currency,
                Status = ProposalStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();

            var createdProposal = await _context.Proposals
                .Include(p => p.CreatedByUser)
                .Include(p => p.UpdatedByUser)
                .Include(p => p.Item)
                .FirstOrDefaultAsync(p => p.Id == proposal.Id);

            if (createdProposal == null)
            {
                throw new InvalidOperationException($"Failed to retrieve created proposal with ID {proposal.Id}");
            }

            return createdProposal;
        }

        public async Task RejectProposalAsync(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);
            if (proposal == null)
            {
                throw new KeyNotFoundException($"Proposal with ID {proposalId} not found.");
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            proposal.Status = ProposalStatus.Rejected;
            proposal.UpdatedAt = DateTime.UtcNow;
            proposal.UpdatedByUserId = currentUser.Id;
            await _context.SaveChangesAsync();
        }

        public async Task AcceptProposalAsync(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);
            if (proposal == null)
            {
                throw new KeyNotFoundException($"Proposal with ID {proposalId} not found.");
            }

            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new InvalidOperationException("Current user not found.");
            }

            proposal.Status = ProposalStatus.Accepted;
            proposal.UpdatedAt = DateTime.UtcNow;
            proposal.UpdatedByUserId = currentUser.Id;
            await _context.SaveChangesAsync();
        }
    }
} 