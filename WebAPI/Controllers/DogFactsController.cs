using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Shared.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogFactsController(IRepository<DogFact> dogFactRepository, ILogger<DogFactsController> logger) : ControllerBase
    {
        private readonly IRepository<DogFact> _dogFactRepository = dogFactRepository;
        private readonly ILogger<DogFactsController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DogFact>>> GetAllDogFacts()
        {
            try
            {
                var facts = await _dogFactRepository.GetAllAsync();
                return Ok(facts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving DogFacts.");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
