namespace Book.Api.Dtos.ShopCarts;

public class ShopCartInputDto
{
    public Guid BookId { get; set; }

    public decimal Size { get; set; }
}
