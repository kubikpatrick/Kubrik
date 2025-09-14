namespace Kubrik.Models.Results;

public sealed class DataResult
{
    private DataResult()
    {
        
    }
    
    public bool Succeeded { get; set; }
    
    public string? Message { get; set; }

    public static DataResult Success() => new DataResult
    {
        Succeeded = true,
        Message = string.Empty
    };

    public static DataResult Fail(string message) => new DataResult
    {
        Succeeded = false,
        Message = message
    };
}