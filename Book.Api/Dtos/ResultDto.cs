namespace Book.Api.Dtos;

public class ResultDto<T>
{
    public bool Success { get; set; }
    public string Errors { get; set; }=string.Empty;

    public T? Data { get; set; }
}
