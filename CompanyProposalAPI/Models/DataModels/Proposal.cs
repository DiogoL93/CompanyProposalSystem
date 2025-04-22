using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProposalAPI.Models.DataModels
{
    public class Proposal : BaseEntity
    {
        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        public int UpdatedByUserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public ProposalValueType ValueType { get; set; }

        [Required]
        public ProposalStatus Status { get; set; } = ProposalStatus.Pending;

        public Currency? Currency { get; set; }

        public string? Description { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public User? UpdatedByUser { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
    }

    public enum ProposalValueType
    {
        Percentage,
        Amount
    }

    public enum ProposalStatus
    {
        Pending,
        Accepted,
        Rejected
    }
} 