using Book.Api.Enums;

namespace Book.Api.Models;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public decimal Price { get; set; }

    public CategoryType Category { get; set; }
}
