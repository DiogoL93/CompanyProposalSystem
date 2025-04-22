using System.ComponentModel.DataAnnotations;
using CompanyProposalAPI.Models.DataModels;

namespace CompanyProposalAPI.Models.Requests
{
    public class CounterProposalRequest : ProposalRequest
    {
        [Required(ErrorMessage = "Description is required for counter proposals")]
        public new string Description { get; set; } = string.Empty;
    }
} 