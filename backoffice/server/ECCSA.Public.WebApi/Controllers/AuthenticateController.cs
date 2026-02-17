using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ECCSA.Public.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : Controller
    {
        protected readonly UserManager<PublicUser> userManager;
        private readonly SignInManager<PublicUser> _signInManager;
        protected ILogger<AuthenticateController> _logger;
        public AuthenticateController(ILogger<AuthenticateController> logger,
            UserManager<PublicUser> userManager,
            IConfiguration configuration,
            SignInManager<PublicUser> signInManager) 
        {
            _logger = logger;
            this.userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: api/<controller>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InputModel model)
         {
            try
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }


                    var jwtSecret = "abcdefghijklmnopqrstuvwxyz1234567890"; //_configuration["JWT:Secret"]
                    var jwtValidIssuer = "http://localhost:44365"; //_configuration["JWT:ValidIssuer"]
                    var jwtValidAudience = "graphql"; //_configuration["JWT:ValidAudience"]

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                    var token = new JwtSecurityToken(
                        issuer: jwtValidIssuer,
                        audience: jwtValidAudience,
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Unauthorized();
        }

    }
    public class InputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public enum Redirects
    {
        Login = 1,
        Lokout = 2,
        Registration = 3
    }
}
