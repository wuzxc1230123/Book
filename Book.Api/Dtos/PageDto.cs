namespace Book.Api.Dtos;

public class PageDto<T>
    where T : class
{
    public List<T> Items { get; set; } =[];

    public int ItemTotal { get; set; } 
}
