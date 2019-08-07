using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuth.Controllers
{
    [Route("api/[controller]")]
    public class OAuthController : Controller
    {
        [HttpPost("token")]
        public async Task<IActionResult> Token(string account, string password)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,account),
                new Claim(ClaimTypes.NameIdentifier,"唯一标识符"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("王盈攀的密钥"));
            var expires = DateTime.Now.AddDays(28);
            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new JsonResult(jwtToken);
        }

        [Authorize]
        [HttpGet("details")]
        public string Details()
        {
            return "Now,You can see the details!";
        }
    }
}
