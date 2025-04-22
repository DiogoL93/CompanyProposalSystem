using CompanyProposalAPI.Models.DataModels;
using CompanyProposalAPI.Models.Requests;

namespace CompanyProposalAPI.Services
{
    public interface IProposalService
    {
        Task<IEnumerable<Proposal>> GetProposalsByItemAsync(int itemId);
        Task<Proposal> CreateProposalWithItemsAsync(ProposalRequest request);
        Task RejectProposalAsync(int proposalId);
        Task AcceptProposalAsync(int proposalId);
    }
} 