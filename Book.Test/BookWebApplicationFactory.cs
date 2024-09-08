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
using Newtonsoft.Json.Linq;
using Book.Api.Dtos.Auths;
using System.Net.Http.Json;
using System.Net;

namespace Book.Test;

public class BookWebApplicationFactory : WebApplicationFactory<Program>,IDisposable
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
        });

        builder.UseEnvironment(Environments.Production);
        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using (var scope = host.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            BookDbContextSeed.InitAsync(scopedServices).Wait();
        }

        return host;
    }
    public HttpClient Client()
    {
        return CreateDefaultClient();
    }


    public static async Task AddToken(HttpClient httpClient,string userName,string password)
    {        //Arrange
        //act
        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = userName,
            Password = password
        });
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadAsStringAsync();

        Assert.NotNull(token);
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
  
}

