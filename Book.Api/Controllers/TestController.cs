using Book.Api.Dtos;
using Book.Api.Models;
using Book.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book.Api.Controllers;


[Route("[controller]/[action]")]
[ApiController]
[Authorize]
public class TestController : ControllerBase
{

    [HttpPost]
    public IActionResult Check()
    {
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string userName = User.FindFirst(ClaimTypes.Name)!.Value;
        IEnumerable<Claim> roleClaims =User.FindAll(ClaimTypes.Role);
        string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
        return Ok($"id={id},userName={userName},roleNames ={roleNames}");
    }
}
