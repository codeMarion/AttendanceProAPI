using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AttendanceProAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuth0Service auth0Service;
        public UserController(IAuth0Service auth0Service)
        {
            this.auth0Service = auth0Service;
        }
        /// <summary>
        /// Updates User Details
        /// </summary>
        /// <param name="user">Updated user object</param>
        /// <returns>Updated User Object</returns>
        /// <response code="200">Returns the updated user object</response>
        /// <response code="400">Unauthorised result</response>
        [HttpPatch]
        public async Task<IActionResult> UpdateDetails([FromBody] UserUpdate user) 
        {
            return await auth0Service.UpdateUserDetails(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, user);
        }
        /// <summary>
        /// Returns the updated user metadata
        /// </summary>
        /// <param name="metadata">New metadata</param>
        /// <response code="200">Returns the user metadata</response>
        /// <response code="400">Unauthorised result</response>
        /// <returns></returns>
        [HttpPatch("{metadata}")]
        public async Task<IActionResult> UpdateMetadata(string metadata)
        {
            return await auth0Service.UpdateUserMetaData(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, metadata == "CLEAR" ? "" : metadata);
        }

        /// <summary>
        /// Retrieves user metadata from the identity provider
        /// </summary>
        /// <response code="200">Returns the user metadata</response>
        /// <response code="400">Unauthorised result</response>
        /// <returns>User metadata</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserMetadata()
        {
            return await auth0Service.GetUserMetaData(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
