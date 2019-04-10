using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CustomerApi.Host.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController : BaseApiController
    {
        [HttpPost("token")]
        public IActionResult GetToken(string role)
        {
            //TODO: Read this from environment variable.
            var key = "this is my security key";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();

            if (role == "reader")
            {
                claims.Add(new Claim(ClaimTypes.Role, "reader"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }

            // Read these from appsettings.json
            var token = new JwtSecurityToken(
                issuer: "Parag",
                audience: "apiUsers",
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: signingCredentials);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
