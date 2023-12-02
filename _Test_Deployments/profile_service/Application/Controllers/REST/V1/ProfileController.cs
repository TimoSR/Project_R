using System.Security.Claims;
using Application.AppServices.V1._Interfaces;
using Application.DataTransferObjects.Profile;
using Domain.Profile.Entities;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClaimTypes = Infrastructure.Middleware.ClaimTypes;

namespace Application.Controllers.REST.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [SwaggerDoc("Profile")]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileServiceV1 _profileService;

        public ProfileController(
            ILogger<ProfileController> logger,
            IProfileServiceV1 profileService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        }
        
        private string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.UserId);
        }


        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProfile([FromBody] ProfileCreateDto profileCreateDto)
        {
            var userId = GetUserId();
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unable to get user ID from token." });
            }
            
            try
            {
                var profile = new Profile
                {
                    UserId = userId,
                    DisplayName = profileCreateDto.DisplayName
                };

                await _profileService.CreateProfileAsync(profile);
                return Ok(new { Message = "Profile successfully created." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create profile: {ex.Message}");
                return BadRequest(new { Message = "An error occurred while creating profile." });
            }
        }

        [HttpGet("info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userId = GetUserId();
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unable to get user ID from token." });
            }
            
            try
            {
                var profileInfo = await _profileService.GetProfileInfoAsync(userId);
                return Ok(profileInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get profile info: {ex.Message}");
                return BadRequest(new { Message = "An error occurred while retrieving profile info." });
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto profileUpdateDto)
        {
            
            var userId = GetUserId();
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Unable to get user ID from token." });
            }
            
            try
            {
                var profile = new Profile
                {
                    UserId = userId,
                    DisplayName = profileUpdateDto.DisplayName,
                    LastLoginDate = profileUpdateDto.LastLoginDate
                };

                await _profileService.UpdateProfileAsync(profile);
                return Ok(new { Message = "Profile successfully updated." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update profile: {ex.Message}");
                return BadRequest(new { Message = "An error occurred while updating profile." });
            }
        }

        [HttpDelete("delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProfile([FromQuery] string userId)
        {
            try
            {
                await _profileService.DeleteProfileAsync(userId);
                return Ok(new { Message = "Profile successfully deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete profile: {ex.Message}");
                return BadRequest(new { Message = "An error occurred while deleting profile." });
            }
        }
    }
}
