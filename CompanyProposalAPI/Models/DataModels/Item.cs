using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProposalAPI.Models.DataModels
{
    public class Item : BaseEntity
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        // Navigation properties
        public required ICollection<Company> Companies { get; set; } = new List<Company>();
        public required ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    }
} 