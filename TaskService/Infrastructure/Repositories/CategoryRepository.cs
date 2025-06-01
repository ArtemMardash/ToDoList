using TaskService.Core.Entities;
using TaskService.Features.Shared.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    public Task<Guid> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategoryAsync(Guid id, string? name, string? description, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}