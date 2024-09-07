using Book.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos.Books;

public class BookPageInputDto : PageInputDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }

    public decimal? Price1 { get; set; }

    public decimal? Price2 { get; set; }

    public CategoryType? Category { get; set; }


}
