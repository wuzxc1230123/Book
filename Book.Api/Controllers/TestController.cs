using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using System.Security.Claims;

namespace Book.Api.Controllers;


[Route("[controller]/[action]")]
[ApiController]
[Authorize]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    private readonly ILogger<TestController> _logger = logger;

    [HttpPost]
    public IActionResult Check()
    {
        _logger.LogInformation("/Test/Check called");

        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string userName = User.FindFirst(ClaimTypes.Name)!.Value;
        IEnumerable<Claim> roleClaims =User.FindAll(ClaimTypes.Role);
        string roleNames = string.Join(',', roleClaims.Select(c => c.Value));


        var baggage1 = Baggage.GetBaggage("Baggage1");

        _logger.LogInformation("Baggage1: {baggage1}", baggage1);

        return Ok($"id={id},userName={userName},roleNames ={roleNames}");
    }
}
