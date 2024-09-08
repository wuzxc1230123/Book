using Book.Api.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Book.Api.Dtos.Books;
using Book.Api.Dtos;

namespace Book.Test;



public class BookApiTest(BookWebApplicationFactory factory) : IClassFixture<BookWebApplicationFactory>
{
    private readonly BookWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Add_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act
        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var response = await httpClient.PostAsJsonAsync("/Book/Add", new BookInputDto()
        {
            Title = "Add_Book",
            Category = Api.Enums.CategoryType.Type1,
            Price = 5
        });
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async Task Update_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act

        var list_Response = await httpClient.GetAsync("/Book/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<BookDto>>())!;

        var book = page.Items.First();

        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var response = await httpClient.PostAsJsonAsync($"/Book/Update?id={book.Id}", new BookInputDto()
        {
            Title = "Update_Book",
            Category = Api.Enums.CategoryType.Type1,
            Price = 5
        });
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act

        var list_Response = await httpClient.GetAsync("/Book/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<BookDto>>())!;

        var book = page.Items.First();

        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var response = await httpClient.PostAsJsonAsync($"/Book/Delete", new DeleteDto()
        {
           Ids=[book.Id]
        });
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task GetList_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act

        var response = await httpClient.GetAsync("/Book/GetList");
       
        //Assert
        //校验状态码
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = (await response.Content.ReadFromJsonAsync<PageDto<BookDto>>())!;

        Assert.True(page.Items.Count>0);

    }
}
