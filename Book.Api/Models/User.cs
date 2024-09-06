using Microsoft.AspNetCore.Identity;

namespace Book.Api.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreationTime { get; set; }
    public string? NickName { get; set; }
    public long JWTVersion { get; set; }
}