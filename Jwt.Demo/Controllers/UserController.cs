using Jwt.Demo.Common;
using Jwt.Demo.Common.Models;
using Jwt.Demo.Persistence;
using Jwt.Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Demo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        public UserController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("/api/users/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest model)
        {
            var user = await _userService.Login(model);

            return Ok(new { token = GenerateAuthToken(user), userInfo = user });
        }

        [HttpGet("/api/users")]
        [Authorize(Policy = ClaimConstants.CANADDUSERS)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpPost("/api/users")]
        [Authorize(Policy = ClaimConstants.CANADDUSERS)]
        public async Task<IActionResult> CreateUser([FromBody] Entities.User model)
        {
            return Ok(await _userService.CreateUser(model));
        }

        [HttpPost("/api/roles")]
        [Authorize(Policy = ClaimConstants.CANADDUSERS)]
        public async Task<IActionResult> CreateRole([FromBody] Entities.Role model)
        {
            return Ok(await _userService.CreateRole(model));
        }

        [HttpGet("/api/roles")]
        [Authorize(Policy = ClaimConstants.CANADDUSERS)]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _userService.GetRoles());
        }

        [HttpGet("/api/posts")]
        public async Task<IActionResult> GetPosts([FromBody] Entities.Role model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method  
                HttpResponseMessage response = await client.GetAsync("posts");

                PostResponse[] posts = null;
                if (response.IsSuccessStatusCode)
                {
                    posts = await response.Content.ReadAsAsync<PostResponse[]>();

                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }

                return Ok(posts);
            }
        }

        private string GenerateAuthToken(Entities.User user)
        {

            var claims = _userService.GetUserClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("AppSettings:Issurer").Value,
                audience: _config.GetSection("AppSettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }

    }
}
