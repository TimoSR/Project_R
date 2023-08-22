using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;

namespace x_endpoints.ControllersREST;

    [Route("api/[controller]")]
    [ApiController]
    public class ArmorController : ControllerBase
    {
        private readonly ArmorService _armorService;

        public ArmorController(ArmorService armorService)
        {
            _armorService = armorService;
        }



        [HttpGet("/Armors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Armor))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Armor>>> Get()
        {
            var armorList = await _armorService.GetAllAsync();
            if (armorList == null)
            {
                NotFound();
            }

            return Ok(armorList);
        }

        [HttpGet("GetArmorByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Armor))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetArmorById(string id)
        {
            var armorToFind = await _armorService.GetByIdAsync(id);
            if (armorToFind == null)
            {
                return NotFound();
            }

            return Ok(armorToFind);

        }
        
        [HttpGet("/Legs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Armor))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Armor>>> GetLegs()
        {
            var armorList = await _armorService.GetAllLegsAsync();
            if (armorList == null)
            {
                NotFound();
            }

            return Ok(armorList);
        }
        [HttpGet("/Heads")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Armor))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Armor>>> GetHeads()
        {
            var armorList = await _armorService.GetAllHeadsAsync();
            if (armorList == null)
            {
                NotFound();
            }

            return Ok(armorList);
        }
        [HttpGet("/Chest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Armor))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Armor>>> GetChest()
        {
            var armorList = await _armorService.GetAllChestAsync();
            if (armorList == null)
            {
                NotFound();
            }

            return Ok(armorList);
        }
    }
