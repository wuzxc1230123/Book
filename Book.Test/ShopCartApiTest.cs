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
using Book.Api.Dtos.ShopCarts;
using Book.Api.Models;

namespace Book.Test;



public class ShopCartApiTest(BookWebApplicationFactory factory) : IClassFixture<BookWebApplicationFactory>
{
    private readonly BookWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Add_OK()
    {
        //Arrange
        var httpClient = _factory.CreateClient();
        //act

        var list_Response = await httpClient.GetAsync("/Book/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<BookDto>>())!;

        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var book= page.Items.First();

        var response = await httpClient.PostAsJsonAsync("/ShopCart/Add", new ShopCartInputDto()
        {
            BookId = book.Id,
            Size = 1
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
        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var list_Response = await httpClient.GetAsync("/ShopCart/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<ShopCartDto>>())!;

        var shopCart = page.Items.First();

        var response = await httpClient.PostAsJsonAsync($"/ShopCart/Update?id={shopCart.Id}", new ShopCartInputDto()
        {
            BookId = shopCart.Book.Id,
            Size = 2
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

        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var list_Response = await httpClient.GetAsync("/ShopCart/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<ShopCartDto>>())!;

        var shopCart = page.Items.First();

        var response = await httpClient.PostAsJsonAsync($"/ShopCart/Delete", new DeleteDto()
        {
            Ids = [shopCart.Id]
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

        var list_Response = await httpClient.GetAsync("/Book/GetList");

        var page = (await list_Response.Content.ReadFromJsonAsync<PageDto<BookDto>>())!;

        await BookWebApplicationFactory.AddToken(httpClient, "Jero123456", "Jero123456");

        var book = page.Items.First();

        await httpClient.PostAsJsonAsync("/ShopCart/Add", new ShopCartInputDto()
        {
            BookId = book.Id,
            Size = 1
        });

        var book2 = page.Items.Take(1).First();

        await httpClient.PostAsJsonAsync("/ShopCart/Add", new ShopCartInputDto()
        {
            BookId = book2.Id,
            Size = 2
        });

        var response = await httpClient.GetAsync("/ShopCart/GetList");
        //Assert
        //校验状态码

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var shopCartPage = (await response.Content.ReadFromJsonAsync<PageDto<ShopCartDto>>())!;
        Assert.True(shopCartPage.Items.Count > 0);

    }
}
