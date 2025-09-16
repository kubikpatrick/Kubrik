using Kubrik.Models.Results;

namespace Kubrik.Api.Services.Managers;

public interface IManager<T>
{
    public Task<T?> FindByIdAsync(string id);

    public Task<DataResult> CreateAsync(T item);
    
    public Task DeleteAsync(T entity);
}