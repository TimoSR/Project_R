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
    public class WoodController : ControllerBase
    {
        private readonly WoodService _woodService;

        public WoodController(WoodService woodService)
        {
            _woodService = woodService;
        }

     

        [HttpGet("/Woods")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Wood))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Wood>>> Get()
        {
            var woodList = await _woodService.GetAllAsync();
            if(woodList == null)
            {
                NotFound();
            }
            return Ok(woodList);
        }

        [HttpGet("GetWoodByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Wood))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetWoodById(string id)
        {
            var woodToFind = await _woodService.GetByIdAsync(id);
            if(woodToFind == null){
                return NotFound();
            }

            return Ok(woodToFind);

        }
       
       
    }
}