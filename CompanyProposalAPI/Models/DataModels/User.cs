using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProposalAPI.Models.DataModels
{
    public class User : BaseEntity
    {
        [Required]
        public required string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        public required string PasswordHash { get; set; }
        
        [Required]
        public required string FirstName { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        [Required]
        public int CompanyId { get; set; }

        // Navigation properties
        [ForeignKey("CompanyId")]
        public required Company Company { get; set; }
    }
} 