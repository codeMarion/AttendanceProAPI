using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPatch]
        public async Task<IActionResult> UpdateDetails([FromBody] UserUpdate user) 
        {
            return await auth0Service.UpdateUserDetails(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, user);
        }

        [HttpPatch("{metadata}")]
        public async Task<IActionResult> UpdateMetadata(string metadata)
        {
            return await auth0Service.UpdateUserMetaData(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, metadata);
        }


        [HttpGet]
        public async Task<IActionResult> GetUserMetadata()
        {
            return await auth0Service.GetUserMetaData(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
