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
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;

namespace Book.Test;

public class BookWebApplicationFactory : WebApplicationFactory<Program>,IDisposable
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
           
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

