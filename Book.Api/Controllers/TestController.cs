using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
