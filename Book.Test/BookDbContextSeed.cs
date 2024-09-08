using Book.Api.Data;
using Book.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Test;

public class BookDbContextSeed(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    private readonly AutoResetEvent _autoEvent = new AutoResetEvent(true);


    public async Task InitAsync()
    {
        _autoEvent.WaitOne();
        var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        await AddLoad(serviceProvider);
        var apiDbContext = serviceProvider.GetRequiredService<ApiDbContext>();

        AddBook(apiDbContext);

        await apiDbContext.SaveChangesAsync();

        _autoEvent.Set();

    }

    private static void AddBook(ApiDbContext apiDbContext)
    {
        for (int i = 1; i < 30; i++)
        {
            var book = new Api.Models.Book()
            {
                Id = Guid.NewGuid(),
                Title = $"Book{i}",
                Author = $"Jero123456",
                Category = i % 2 == 0 ? Api.Enums.CategoryType.Type1 : Api.Enums.CategoryType.Type2,
                Price = i,
            };
            apiDbContext.Add(book);
        }
    }

    private static async Task AddLoad(IServiceProvider serviceProvider)
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
