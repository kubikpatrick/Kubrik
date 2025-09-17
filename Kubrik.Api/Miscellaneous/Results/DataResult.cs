namespace Kubrik.Api.Miscellaneous.Results;

public sealed class DataResult
{
    private DataResult()
    {
        
    }
    
    public bool Succeeded { get; set; }
    
    public int HttpCode { get; set; }
    
    public string? Message { get; set; }

    public static DataResult Success() => new DataResult
    {
        Succeeded = true,
        HttpCode = StatusCodes.Status200OK,
        Message = string.Empty
    };

    public static DataResult Fail(string message, int code) => new DataResult
    {
        Succeeded = false,
        HttpCode = code,
        Message = message
    };
}