using AddNumbers.API.Extensions;
using AddNumbers.API.Models;
using AddNumbers.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddNumbers.API.Controllers
{
    /// <summary>
    /// API controller that handles number-related operations stored in session.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NumbersController : ControllerBase
    {
        private readonly ISessionNumberService _numberService;
        private readonly ILogger<NumbersController> _logger;

        public NumbersController(ISessionNumberService numberService, ILogger<NumbersController> logger)
        {
            _numberService = numberService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the list of numbers and their count.
        /// </summary>
        /// <returns>List of numbers and total count.</returns>
        [HttpGet]
        public ActionResult<NumberListResponse> GetNumbers()
        {
            try
            {
                return new NumberListResponse
                {
                    Numbers = _numberService.GetAll(),
                    Count = _numberService.GetCount()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting numbers");
                return StatusCode(500, new { error = "Something went wrong." });
            }
        }


        /// <summary>
        /// Adds a random number to the list.
        /// </summary>
        /// <returns>The newly added number.</returns>
        [HttpPost]
        public ActionResult<AddNumberResponse> AddNumber()
        {
            try
            {
                int added = _numberService.AddRandom();
                return new AddNumberResponse { Added = added };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a number");
                return StatusCode(500, new { error = "Failed to add number." });
            }
        }

        /// <summary>
        /// Clears all numbers from session.
        /// </summary>
        [HttpDelete]
        public IActionResult ClearNumbers()
        {
            try
            {
                _numberService.Clear();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing numbers");
                return StatusCode(500, new { error = "Failed to clear numbers." });
            }
        }

        /// <summary>
        /// Gets the sum of all numbers in session.
        /// </summary>
        /// <returns>The sum of numbers.</returns>
        [HttpGet("sum")]
        public ActionResult<SumResponse> GetSum()
        {
            try
            {
                return new SumResponse { Sum = _numberService.GetSum() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating sum");
                return StatusCode(500, new { error = "Failed to calculate sum." });
            }
        }
    }
}
