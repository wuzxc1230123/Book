using Book.Api.Dtos.Auths;
using Book.Api.Models;
using Book.Api.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class AuthController(UserManager<User> userManager, IOptions<JWTOptions> jwtOptions) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IOptions<JWTOptions> _jwtOptions = jwtOptions;

    [HttpPost]
    public async Task<IActionResult> Login(LoginInputDto input)
    {
        string userName = input.UserName;
        string password = input.Password;
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound($"用户名不存在{userName}");
        }
        var success = await _userManager.CheckPasswordAsync(user, password);
        if (!success)
        {
            return BadRequest("Failed");
        }
        user.JWTVersion++;
        await _userManager.UpdateAsync(user);
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.UserName!),
            new (ClaimTypes.Version, user.JWTVersion.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        string jwtToken = BuildToken(claims, _jwtOptions.Value);
        return Ok(jwtToken);
    }

    private static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
    {
        DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
        byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
        var secKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(secKey,
            SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(expires: expires,
            signingCredentials: credentials, claims: claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }



}
