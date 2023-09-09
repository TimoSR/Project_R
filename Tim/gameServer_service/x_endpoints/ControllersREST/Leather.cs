using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;

namespace x_endpoints.ControllersREST
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeatherController : ControllerBase
    {
        private readonly LeatherSerivce _leatherService;

        public LeatherController(LeatherSerivce leatherService)
        {
            _leatherService = leatherService;
        }



        [HttpGet("/Leathers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Leather))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Leather>>> Get()
        {
            var leatherList = await _leatherService.GetAllAsync();
            if (leatherList == null)
            {
                NotFound();
            }

            return Ok(leatherList);
        }

        [HttpGet("GetLeatherByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Leather))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetLeatherById(string id)
        {
            var leatherToFind = await _leatherService.GetByIdAsync(id);
            if (leatherToFind == null)
            {
                return NotFound();
            }

            return Ok(leatherToFind);

        }
    }
}