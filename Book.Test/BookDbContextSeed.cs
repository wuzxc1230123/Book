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

public class BookDbContextSeed
{

    public static async Task InitAsync(IServiceProvider serviceProvider)
    {
        var apiDbContext = serviceProvider.GetRequiredService<ApiDbContext>();

        AddBook(apiDbContext);

        await apiDbContext.SaveChangesAsync();
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

}
