namespace Kubrik.Api.Services.Contracts;

public interface ICurrentUserIdAccessor
{
    public string UserId { get; }
}