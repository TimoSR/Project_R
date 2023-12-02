
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Persistence.ServiceRegistration;


namespace x_endpoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly WeaponService _weaponService;

        public WeaponController(WeaponService weaponService)
        {
            _weaponService = weaponService;
        }

     

        [HttpGet("/Weapons")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Weapon))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<ActionResult<IEnumerable<Weapon>>> Get()
        {
            var weaponList = await _weaponService.GetAllAsync();
            if(weaponList == null)
            {
                NotFound();
            }
            return Ok(weaponList);
        }

        [HttpGet("GetWeaponByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Weapon))] // Success response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found response
        public async Task<IActionResult> GetWeaponById(string id)
        {
            var weaponToFind = await _weaponService.GetByIdAsync(id);
            if(weaponToFind == null){
                return NotFound();
            }

            return Ok(weaponToFind);

        }
       
       
    }
}