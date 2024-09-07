using Book.Api.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Book.Test;

public abstract class BookApiTestBase : IClassFixture<BookWebApplicationFactory>
{
    public static async Task AddToken(HttpClient httpClient, string userName,string password)
    {
        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = userName,
            Password = password
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadAsStringAsync();

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

    }
}
