using Book.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.Data;


/// <summary>
/// EF
/// 
/// </summary>
/// <param name="options"></param>
public class ApiDbContext(DbContextOptions<ApiDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
//Add-migration initIndetity
//update-database
