using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProposalAPI.Models.DataModels
{
    public class Company : BaseEntity
    {
        [Required]
        public required string Name { get; set; }

        // Navigation properties
        public required ICollection<User> Users { get; set; } = new List<User>();
        public required ICollection<Item> Items { get; set; } = new List<Item>();
    }
} 