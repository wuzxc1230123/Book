using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos.Auths;

public class LoginInputDto
{
    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
