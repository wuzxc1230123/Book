using System.Reflection.Metadata;

namespace Book.Api.Models;

public class ShopCart
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public Guid UserId { get; set; }

    public decimal Size { get; set; }


    public Book Book { get; set; } = null!;
}
