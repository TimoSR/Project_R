using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;

namespace x_endpoints.ControllersREST
{
    [Route("api/[controller]")]
    [ApiController]
    public class HideController : ControllerBase
    {
        private readonly HideService _hideService;

        public HideController(HideService hideService)
        {
            _hideService = hideService;
        }



        [HttpGet("/Hides")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Hide))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Hide>>> Get()
        {
            var hideList = await _hideService.GetAllAsync();
            if (hideList == null)
            {
                NotFound();
            }

            return Ok(hideList);
        }

        [HttpGet("GetHideByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Hide))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetHideById(string id)
        {
            var hideToFind = await _hideService.GetByIdAsync(id);
            if (hideToFind == null)
            {
                return NotFound();
            }

            return Ok(hideToFind);

        }
    }
}