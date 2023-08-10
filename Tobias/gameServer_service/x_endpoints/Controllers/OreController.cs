using Microsoft.AspNetCore.Mvc;
using x_endpoints.Models;
using System.Collections.Generic;
using System.Net;
using x_endpoints.Services;


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

        // POST api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Ore))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Ore ore)
        {
            if (ModelState.IsValid)
            {
                await _oreService.InsertProduct(ore);
                return CreatedAtAction(nameof(GetOreById), new { id = ore.Id }, ore); // Return 201 Created
            }
            
            return BadRequest(ModelState); // Return 400 Bad Request
        }

        [HttpGet("/Ores")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ore))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Ore>>> Get()
        {
            var oreList = await _oreService.GetAsync();
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
            var oreToFind = await _oreService.GetOreByIdAsync(id);
            if(oreToFind == null){
                return NotFound();
            }

            return Ok(oreToFind);

        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Ore))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Ore))] // Success response
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _oreService.DeleteOreAsync(id);
            
            if (deleted)
            {
                return NoContent(); // Successfully deleted
            }
            else
            {
                return NotFound(); // ID not found
            }
        }
        // public OreController(OreService oreService)
        // {
        //     _oreService = oreService;
        // }

        // [HttpGet]
        // public ActionResult<IEnumerable<Ore>> Get()
        // {
        //     var ores = _oreService.GetOres();
        //     return Ok(ores);
        // }

        // [HttpGet("{id:length(24)}", Name = "GetOre")]
        // public ActionResult<Ore> Get(string id)
        // {
        //     var ore = _oreService.GetOreById(id);
        //     if (ore == null)
        //     {
        //         return NotFound("Ore not found.");
        //     }
        //     return Ok(ore);
        // }

       

        // [HttpPut("Update/{id:length(24)}")]
        // public Task<IActionResult> Update(string id,[FromBody] Ore updatedOre)
        // {
        //     var ore = _oreService.GetOreById(id);
        //     if (ore == null)
        //     {
        //          return NotFound("Ore not found.");
        //     }
        //
        //     _oreService.UpdateOre(id, updatedOre);
        //     return NoContent();
        //  }

        // [HttpDelete("{id:length(24)}")]
        // public IActionResult Delete(string id)
        // {
        //     var ore = _oreService.GetOreById(id);
        //     if (ore == null)
        //     {
        //         return NotFound("Ore not found.");
        //     }

        //     _oreService.DeleteOre(id);
        //     return NoContent();
        // }
    }
}