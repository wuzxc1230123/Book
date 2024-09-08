using Book.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book.Api.Data;

public class DbContextSeed
{
    public async Task InitAsync(IServiceProvider serviceProvider)
    {
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

        var user = new User()
        {
            UserName = "Jero123456",
            Id = Guid.NewGuid()
        };
        var userResult = await userManager.CreateAsync(user, "Jero123456");

        var addResult = await userManager.AddToRoleAsync(user, RoleName);
    }
}
