using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos.ShopCarts;

public class ShopCartInputDto
{
    [Required]
    public Guid BookId { get; set; }
    [Required]
    public decimal Size { get; set; }
}
