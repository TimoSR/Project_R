using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Persistence.ServiceRegistration;


namespace x_endpoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OreController : ControllerBase
    {
        private readonly OreService _oreService;

        public OreController(OreService oreService)
        {
            _oreService = oreService;
        }

     

        [HttpGet("/Ores")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ore))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Ore>>> Get()
        {
            var oreList = await _oreService.GetAllAsync();
            if(oreList == null)
            {
                NotFound();
            }
            return Ok(oreList);
        }

        [HttpGet("GetOreByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ore))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetOreById(string id)
        {
            var oreToFind = await _oreService.GetByIdAsync(id);
            if(oreToFind == null){
                return NotFound();
            }

            return Ok(oreToFind);

        }
       
       
    }
}