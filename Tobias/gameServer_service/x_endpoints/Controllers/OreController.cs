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
        public async Task<IActionResult> Post([FromBody] Ore ore)
        {
            await _oreService.InsertProduct(ore);
            
            return Ok(ore);
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

        // [HttpPost]
        // public ActionResult<Ore> Create(Ore ore)
        // {
        //     var createdOre = _oreService.CreateOre(ore);
        //     return CreatedAtRoute("GetOre", new { id = createdOre.Id }, createdOre);
        // }

        // [HttpPut("{id:length(24)}")]
        // public IActionResult Update(string id, Ore updatedOre)
        // {
        //     var ore = _oreService.GetOreById(id);
        //     if (ore == null)
        //     {
        //         return NotFound("Ore not found.");
        //     }

        //     _oreService.UpdateOre(id, updatedOre);
        //     return NoContent();
        // }

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