using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Book.Api.Dtos.Auths;

namespace Book.Test;

public class AuthApiTest(BookWebApplicationFactory factory) : BookApiTestBase
{
    private readonly BookWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Login_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act
        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = "Jero123456",
            Password= "Jero123456"
        });
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadAsStringAsync();

        Assert.NotNull(token);
    }

    [Fact]
    public async Task Login_Found()
    {
        var httpClient = _factory.CreateClient();

        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = "Jero123456a",
            Password = "Jero123456a"
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Login_BadRequest()
    {
        var httpClient = _factory.CreateClient();

        var response = await httpClient.PostAsJsonAsync("/Auth/Login", new LoginInputDto()
        {
            UserName = "Jero123456",
            Password = "Jero123456a"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }
}
