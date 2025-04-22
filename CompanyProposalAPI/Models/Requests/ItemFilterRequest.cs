using System.ComponentModel.DataAnnotations;

namespace CompanyProposalAPI.Models.Requests
{
    public class ItemFilterRequest
    {
        public string? Name { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
} 