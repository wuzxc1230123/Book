using Book.Api.Enums;

namespace Book.Api.Dtos.Books;

public class BookDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public decimal Price { get; set; }

    public CategoryType Category { get; set; }
}
