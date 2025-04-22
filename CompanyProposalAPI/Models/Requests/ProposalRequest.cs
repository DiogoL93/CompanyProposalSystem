using System.ComponentModel.DataAnnotations;
using CompanyProposalAPI.Models.DataModels;

namespace CompanyProposalAPI.Models.Requests
{
    public class ProposalRequest
    {
        public string? Description { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public ProposalValueType ValueType { get; set; }

        public Currency? Currency { get; set; }
    }
} 