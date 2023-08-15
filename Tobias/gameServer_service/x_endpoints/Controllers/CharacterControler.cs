using Microsoft.AspNetCore.Mvc;
using x_endpoints.Models;
using System.Collections.Generic;
using System.Net;
using x_endpoints.Services;

namespace x_endpoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly BaseService<Character> _characterService;

        public PlayerController(BaseService<Character> characterService)
        {
            _characterService = characterService;
        }

        // POST api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Character))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Character player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _characterService.InsertData(player);
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player); // Return 201 Created
        }

        [HttpGet("/Players")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Character))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Character>>> Get()
        {
            var characterList = await _characterService.GetAllAsync();
            if(characterList == null)
            {
                NotFound();
            }
            return Ok(characterList);
        }

        [HttpGet("GetPlayerByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Character))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetPlayerById(string id)
        {
            var characterToFind = await _characterService.GetByIdAsync(id);
            if(characterToFind == null){
                return NotFound();
            }

            return Ok(characterToFind);

        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Character))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Character))] // Success response
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _characterService.DeleteAsync(id);
            
            if (!deleted)
            {
                return NoContent(); // Successfully deleted
            }
            return NotFound(); // ID not found
            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Character))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Character))] // Success response
        public async Task<IActionResult> UpdateData(string id, [FromBody] Character updatedPlayer) // Change DataModel to your actual model class
        {
            var existingData = await _characterService.GetByIdAsync(id);

            if (existingData == null)
            {
                return NotFound();
            }

            // You can implement any necessary validation or business logic here
            // Update the properties of existingData with the properties of updatedData
            // ...

            await _characterService.UpdateAsync(id, existingData);

            return NoContent();
        }
       
    }
}