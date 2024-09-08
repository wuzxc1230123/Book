using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Book.Api.Dtos.Auths;
using Microsoft.AspNetCore.Mvc.Testing;
using Book.Api.Dtos.Books;
using Book.Api.Dtos.ShopCarts;
using Book.Api.Dtos;

namespace Book.Test;



public class CheckoutApiTest(BookWebApplicationFactory factory) : IClassFixture<BookWebApplicationFactory>
{
    private readonly BookWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Calculate_OK()
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

        var response = await httpClient.GetAsync("/Checkout/Calculate");
        //Assert
        //校验状态码

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var calculate1 = book2.Price * 2 + book.Price * 1;
        var calculate2 = decimal.Parse(await response.Content.ReadAsStringAsync());


        Assert.Equal(calculate1, calculate2);
    }
}
