namespace Book.Api.Options;

public class JWTOptions
{
    public string SigningKey { get; set; } = "faaasdasdasdsadasdsadsadasdasdsadsadfadpio@0232";
    public int ExpireSeconds { get; set; } = 300;
}