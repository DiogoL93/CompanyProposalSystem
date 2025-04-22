using Microsoft.AspNetCore.Mvc;

namespace CompanyProposalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;

        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected ActionResult<T> HandleException<T>(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request");
            return StatusCode(500, "An internal server error occurred.");
        }

        protected IActionResult HandleException(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
} 