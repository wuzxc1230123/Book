using Book.Api.Dtos.Books;

namespace Book.Api.Dtos.ShopCarts
{
    public class ShopCartDto
    {
        public Guid Id { get; set; }

        public BookDto Book { get; set; }

        public Guid UserId { get; set; }

        public decimal Size { get; set; }
    }
}
