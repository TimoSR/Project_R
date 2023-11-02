using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;

namespace x_endpoints.ControllersREST;

    [Route("api/[controller]")]
    [ApiController]
    public class PickAxeController : ControllerBase
    {
        private readonly PickaxeService _pickAxeService;

        public PickAxeController(PickaxeService pickAxeService)
        {
            _pickAxeService = pickAxeService;
        }



        [HttpGet("/PickAxes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pickaxe))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Pickaxe>>> Get()
        {
            var pickAxeList = await _pickAxeService.GetAllAsync();
            if (pickAxeList == null)
            {
                NotFound();
            }

            return Ok(pickAxeList);
        }

        [HttpGet("GetPickAxeByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pickaxe))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetPickAxeById(string id)
        {
            var pickAxeToFind = await _pickAxeService.GetByIdAsync(id);
            if (pickAxeToFind == null)
            {
                return NotFound();
            }

            return Ok(pickAxeToFind);

        }
        
        
    }
