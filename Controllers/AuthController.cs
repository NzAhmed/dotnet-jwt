using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
/***********************************************************/
using Microsoft.IdentityModel.Tokens ;
// System.IdentityModel.Tokens.Jwt for JwtSecurityToken & JwtSecurityTokenHandler
// dotnet add package System.IdentityModel.Tokens.Jwt 
using System.IdentityModel.Tokens.Jwt;
/***********************************************************/

using webapi2.Models;

namespace webapi2.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // login
        // curl -X POST https://localhost:5001/api/auth/login -d "{\"userName\":\"nz\",\"password\":\"ahmed\"}" -H "Content-Type: application/json" -k

        // 
        // curl -X GET -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1Njc4Njk0NTAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0.iRqpIr-UYmP1ySX6qzzk9ay9BKyfu0lAfI8VHQKy5c0" -H "Cache-Control: no-cache" "https://localhost:5001/api/values" -k
        [HttpPost, Route("login")]
        public IActionResult Login(LoginModel user)
        {          
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
        
            if (user.UserName == "nz" && user.Password == "ahmed")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                //claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });

            }
            else
            {
                return Unauthorized();
            }
        }



        private string GenerateJWT()
        {
            var issuer = "https://localhost:5001/";
            var audience =  "https://localhost:5001/";
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zzzzzzzz"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer, 
                audience:audience,
                expires: DateTime.Now.AddMinutes(120), 
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

    }
}