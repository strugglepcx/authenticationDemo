using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using authenticationWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace authenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController : Controller
    {
        private readonly JwtSettings _jwtSettings;

        public AuthorizeController(IOptions<JwtSettings> jwtSettingsAccessor)
        {
            _jwtSettings = jwtSettingsAccessor.Value;
        }

        [HttpPost]
        [Route("Token")]
        public IActionResult Token(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!(viewModel.UserName == "strugglepcx" && viewModel.Password == "123456"))
                {
                    return BadRequest();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "strugglepcx"),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, DateTime.Now,
                    DateTime.Now.AddMinutes(30), creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest();
        }
    }
}
