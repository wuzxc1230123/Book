using Book.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Book.Api.Data;

public class DbContextSeed(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;



    public async Task InitAsync()
    {
        var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;


        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var RoleName = "admin";
        var roleHas = await roleManager.RoleExistsAsync(RoleName);
        if (roleHas)
        {
            return;
        }
        var role = new Role()
        {
            Name = RoleName,
            Id = Guid.NewGuid()
        };
        var roleResutl = await roleManager.CreateAsync(role);
        if (!roleResutl.Succeeded)
        {
            return;
        }
        var olduser = await userManager.FindByNameAsync("Jero");
        if (olduser == null)
        {
            var user = new User()
            {
                UserName = "Jero123456", 
                Id = Guid.NewGuid()
            };
            var userResult = await userManager.CreateAsync(user, "Jero123456");
            if (!userResult.Succeeded)
            {
                return;
            }
            var isinRole = await userManager.IsInRoleAsync(user, RoleName);
            if (!isinRole)
            {
                var addResult = await userManager.AddToRoleAsync(user, RoleName);
                if (!addResult.Succeeded)
                {
                    return;
                }
            }
        }
    }
}
