using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.Data;


/// <summary>
/// EF core
/// </summary>
/// <param name="opt"></param>
public class ApiDbContext(DbContextOptions<ApiDbContext> opt) : IdentityDbContext(opt)
{
}
