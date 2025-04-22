using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompanyProposalAPI.Models.DataModels;
using CompanyProposalAPI.Services;
using CompanyProposalAPI.Models.Requests;

namespace CompanyProposalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagingController : BaseController
    {
        private readonly IProposalService _proposalService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public ManagingController(IProposalService proposalService, ILogger<ManagingController> logger, IUserService userService, ICompanyService companyService)
            : base(logger)
        {
            _proposalService = proposalService;
            _userService = userService;
            _companyService = companyService;
        }

        
        [HttpGet("proposals/{itemId}")]
        public async Task<ActionResult<IEnumerable<string>>> ViewProposals(int itemId)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized("User not found");
                }

                var proposals = await _proposalService.GetProposalsByItemAsync(itemId);
                var formattedProposals = new List<string>();

                foreach (var proposal in proposals)
                {
                    var user = proposal.UpdatedByUser ?? proposal.CreatedByUser;
                    var company = user?.Company;
                    var status = proposal.Status.ToString();

                    string formattedString;
                    if (user?.CompanyId == currentUser.CompanyId)
                    {
                        formattedString = $"{status} by {user?.FirstName} {user?.LastName} on behalf of {company?.Name}";
                    }
                    else
                    {
                        formattedString = $"{status} by {company?.Name}";
                    }

                    formattedProposals.Add(formattedString);
                }

                return Ok(formattedProposals);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<string>>(ex);
            }
        }

        [HttpPost("proposal")]
        public async Task<ActionResult<Proposal>> CreateProposal([FromBody] ProposalRequest request)
        {
            try
            {
                var proposal = await _proposalService.CreateProposalWithItemsAsync(request);
                return CreatedAtAction(nameof(ViewProposals), new { itemId = proposal.ItemId }, proposal);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException<Proposal>(ex);
            }
        }

        [HttpPost("proposal/{proposalId}/counter")]
        public async Task<ActionResult<Proposal>> CounterProposal(int proposalId, [FromBody] CounterProposalRequest request)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized("User not found");
                }

                var proposal = await _proposalService.GetProposalsByItemAsync(request.ItemId);
                var existingProposal = proposal.FirstOrDefault(p => p.Id == proposalId);
                
                if (existingProposal == null)
                {
                    return NotFound($"Proposal with ID {proposalId} not found.");
                }

                if (existingProposal.CreatedByUserId == currentUser.Id)
                {
                    return BadRequest("You cannot create a counter proposal to your own proposal.");
                }

                // First reject the existing proposal
                await _proposalService.RejectProposalAsync(proposalId);
                
                // Create the counter proposal using the same method as regular proposals
                var counterProposal = await _proposalService.CreateProposalWithItemsAsync(request);
                return CreatedAtAction(nameof(ViewProposals), new { itemId = counterProposal.ItemId }, counterProposal);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException<Proposal>(ex);
            }
        }

        [HttpPut("proposal/{proposalId}/accept")]
        public async Task<IActionResult> AcceptProposal(int proposalId)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized("User not found");
                }

                var proposal = await _proposalService.GetProposalsByItemAsync(proposalId);
                var existingProposal = proposal.FirstOrDefault(p => p.Id == proposalId);
                
                if (existingProposal == null)
                {
                    return NotFound($"Proposal with ID {proposalId} not found.");
                }

                if (existingProposal.CreatedByUserId == currentUser.Id)
                {
                    return BadRequest("You cannot accept your own proposal.");
                }

                await _proposalService.AcceptProposalAsync(proposalId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("items")]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCompanyItems([FromQuery] ItemFilterRequest filter)
        {
            try
            {
                // Get the current user's company ID
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized("User not found");
                }

                // Get all items for the company
                var items = await _companyService.GetCompanyItemsAsync(currentUser.CompanyId);

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    items = items.Where(i => i.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (filter.CreatedAfter.HasValue)
                {
                    items = items.Where(i => i.CreatedAt >= filter.CreatedAfter.Value);
                }

                if (filter.CreatedBefore.HasValue)
                {
                    items = items.Where(i => i.CreatedAt <= filter.CreatedBefore.Value);
                }

                if (filter.MinPrice.HasValue)
                {
                    items = items.Where(i => i.Price >= filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    items = items.Where(i => i.Price <= filter.MaxPrice.Value);
                }

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                {
                    items = filter.SortBy.ToLower() switch
                    {
                        "name" => filter.SortDescending ? items.OrderByDescending(i => i.Name) : items.OrderBy(i => i.Name),
                        "price" => filter.SortDescending ? items.OrderByDescending(i => i.Price) : items.OrderBy(i => i.Price),
                        "createdat" => filter.SortDescending ? items.OrderByDescending(i => i.CreatedAt) : items.OrderBy(i => i.CreatedAt),
                        _ => items
                    };
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 