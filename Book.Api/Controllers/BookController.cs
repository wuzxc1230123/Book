using Book.Api.Data;
using Book.Api.Dtos;
using Book.Api.Dtos.Books;
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


[ApiController]
[Route("[controller]/[action]")]
public class BookController(ApiDbContext apiDbContext) : ControllerBase
{
    private readonly ApiDbContext _apiDbContext = apiDbContext;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(BookInputDto input)
    {
        if (await _apiDbContext.Books.AnyAsync(a => a.Title == input.Title))
        {
            return NotFound($"标题存在{input.Title}");
        }
        string userName = User.FindFirst(ClaimTypes.Name)!.Value;

        var book = new Models.Book()
        {
            Id = Guid.NewGuid(),
            Title = input.Title,
            Author = userName,
            Category = input.Category,
            Price = input.Price,
        };
        _apiDbContext.Add(book);
        await _apiDbContext.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(Guid id, BookInputDto input)
    {
        if (await _apiDbContext.Books.AnyAsync(a => a.Id != id && a.Title == input.Title))
        {
            return NotFound($"标题存在{input.Title}");
        }

        var book = await _apiDbContext.Books.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (book == null)
        {
            return NotFound($"编号不存在{id}");
        }
        string userName = User.FindFirst(ClaimTypes.Name)!.Value;
        book.Author = userName;
        book.Category = input.Category;
        book.Price = input.Price;

        _apiDbContext.Update(book);
        await _apiDbContext.SaveChangesAsync();
        return Ok();
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteDto input)
    {

        var books = await _apiDbContext.Books.Where(a => input.Ids.Contains(a.Id)).ToListAsync();

        foreach (var book in books)
        {
            _apiDbContext.Remove(book);
        }
        await _apiDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetList(BookPageInputDto input)
    {
        var books = _apiDbContext.Books.AsNoTracking();
        if (!string.IsNullOrEmpty(input.Title))
        {
            books = books.Where(a => a.Title.Contains(input.Title));
        }
        if (!string.IsNullOrEmpty(input.Author))
        {
            books = books.Where(a => a.Title.Contains(input.Author));
        }
        if (input.Price1 != null)
        {
            books = books.Where(a => a.Price >= input.Price1);
        }
        if (input.Price2 != null)
        {
            books = books.Where(a => a.Price <= input.Price1);
        }
        if (input.Category != null)
        {
            books = books.Where(a => a.Category == input.Category);
        }
        var count = await books.CountAsync();
        if (count == 0)
        {
            return Ok(new PageDto<BookDto>()
            {
                ItemTotal = 0,
            });
        }
        var items = await books.Skip((input.PageNum - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        return Ok(new PageDto<BookDto>()
        {
            ItemTotal = count,
            Items = items.Select(a => new BookDto()
            {
                Id = a.Id,
                Title = a.Title,
                Author = a.Author,
                Category = a.Category,
                Price = a.Price,
            }).ToList()
        });
    }
}
