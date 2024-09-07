using Book.Api.Data;
using Book.Api.Dtos;
using Book.Api.Dtos.Books;
using Book.Api.Dtos.ShopCarts;
using Book.Api.Models;
using Book.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Book.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class ShopCartController(ApiDbContext apiDbContext) : ControllerBase
{
    private readonly ApiDbContext _apiDbContext = apiDbContext;

    [HttpPost]
    public async Task<IActionResult> Add(ShopCartInputDto input)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var shopCart = await _apiDbContext.ShopCarts.Where(a => a.UserId == userId && a.BookId == input.BookId).FirstOrDefaultAsync();
        if (shopCart == null)
        {
            shopCart = new Models.ShopCart()
            {
                Id = Guid.NewGuid(),
                BookId = input.BookId,
                UserId = userId,
                Size = input.Size,
            };
            _apiDbContext.Add(shopCart);

        }
        else
        {
            shopCart.Size += input.Size;
            _apiDbContext.Update(shopCart);
        }

        await _apiDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid id, ShopCartInputDto input)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var shopCart = await _apiDbContext.ShopCarts.Where(a => a.Id == id && input.BookId == input.BookId).FirstOrDefaultAsync();
        if (shopCart == null)
        {
            return NotFound($"编号不存在{id}");
        }
        shopCart.Size += input.Size;
        _apiDbContext.Update(shopCart);
        await _apiDbContext.SaveChangesAsync();

        return Ok();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(DeleteDto input)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var shopCarts = await _apiDbContext.ShopCarts.Where(a => input.Ids.Contains(a.Id)&& a.UserId== userId).ToListAsync();

        foreach (var shopCart in shopCarts)
        {
            _apiDbContext.Remove(shopCart);
        }
        await _apiDbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetList(ShopCartPageInputDto input)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var shopCarts = _apiDbContext.ShopCarts.AsNoTracking();
        shopCarts = shopCarts.Where(a => a.UserId == userId);
        if (!string.IsNullOrEmpty(input.Title))
        {
            shopCarts = shopCarts.Where(a => a.Book.Title.Contains(input.Title));
        }

        var count = await shopCarts.CountAsync();
        if (count == 0)
        {
            return Ok(new PageDto<BookDto>()
            {
                ItemTotal = 0,
            });
        }
        var items = await shopCarts.Skip((input.PageNum - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        return Ok(new PageDto<ShopCartDto>()
        {
            ItemTotal = count,
            Items = items.Select(a => new ShopCartDto()
            {
                Id = a.Id,
                Size = a.Size,
                UserId = a.UserId,
                Book = new BookDto()
                {
                    Id = a.Book.Id,
                    Author = a.Book.Author,
                    Category = a.Book.Category,
                    Price = a.Book.Price,
                    Title = a.Book.Title,
                }
            }).ToList()
        });
    }
}
