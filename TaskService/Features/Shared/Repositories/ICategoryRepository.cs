using TaskService.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Features.Shared.Repositories;

public interface ICategoryRepository
{
    /// <summary>
    /// Create category 
    /// </summary>
    public Task<Guid> CreateCategoryAsync(Category category, CancellationToken cancellationToken);

    /// <summary>
    /// Get category by id
    /// </summary>
    public Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get category by name
    /// </summary>
    public Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken);
    
    /// <summary>
    /// Update category
    /// </summary>
    public Task UpdateCategoryAsync(Guid id, string? name, string? description, CancellationToken cancellationToken);

    /// <summary>
    /// Delete category
    /// </summary>
    public Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken);
}