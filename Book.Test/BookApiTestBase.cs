using Book.Api.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Book.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Book.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book.Test;

[CollectionDefinition("Book")]
public abstract class BookApiTestBase : ICollectionFixture<BookWebApplicationFactory>
{
    public static async Task AddToken(HttpClient httpClient, string userName, string password)
    {
        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = userName,
            Password = password
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadAsStringAsync();

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    }


}
