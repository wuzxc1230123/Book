using Book.Api.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Book.Api.Dtos.Books;

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
        //await AddToken(httpClient, "Jero123456", "Jero123456");

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
}
