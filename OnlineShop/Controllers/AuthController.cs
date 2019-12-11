using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;

namespace OnlineShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(UserService userService)
        {
            UserService = userService;
        }

        public UserService UserService { get; }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var token = await UserService.Authenticate(phoneNumber);

            if (String.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }

        [HttpPost]
        public async Task<IActionResult> Registrate(string fullName, string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var token = await UserService.Registrate(fullName, phoneNumber);

            if (String.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new {token});
        }
    }
}