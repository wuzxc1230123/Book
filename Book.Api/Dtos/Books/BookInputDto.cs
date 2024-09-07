using Book.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos.Books;

public class BookInputDto
{
    [Required]
    public string Title { get; set; } = null!;
   
    [Required]
    public decimal Price { get; set; }
    [Required]
    public CategoryType Category { get; set; }
}
