using Book.Api.Data;
using Book.Api.Dtos.Books;
using Book.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.Controllers;


[Route("[controller]/[action]")]
[ApiController]
public class CheckoutController(ApiDbContext apiDbContext) : ControllerBase
{
    private readonly ApiDbContext _apiDbContext = apiDbContext;


    [HttpGet]
    public async Task<IActionResult> Calculate()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var shopCarts = _apiDbContext.ShopCarts.AsNoTracking();
        shopCarts = shopCarts.Where(a => a.UserId == userId);
        var sum = await shopCarts.SumAsync(a => a.Size * a.Book.Price);
        return Ok(sum);
    }
}
