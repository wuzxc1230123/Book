using System.ComponentModel.DataAnnotations;

namespace Book.Api.Dtos.Auth.Request;

public class TokenRequestDto
{
    [Required]
    public string Token { get; set; }=string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
