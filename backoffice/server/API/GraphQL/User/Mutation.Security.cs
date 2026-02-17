using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECCSA.BackOffice.Server.Model.Security;


namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {

        public async Task<UserToken> Login(InputModel model,
            [Service] UserManager<User> userManager)
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

                    return new UserToken
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new Exception("Unauthorized");
        }

        public class InputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserToken
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
        }

    }
}
