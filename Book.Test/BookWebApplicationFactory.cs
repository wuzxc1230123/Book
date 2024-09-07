using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Book.Api.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Book.Test;

public class BookWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //services.AddSingleton<BookDbContextSeed>();
        });

        builder.UseEnvironment("Test");
        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        //host.Services.GetRequiredService<BookDbContextSeed>().InitAsync().Wait();
        return host;
    }
    public HttpClient Client()
    {
        return CreateDefaultClient();
    }

   
}

