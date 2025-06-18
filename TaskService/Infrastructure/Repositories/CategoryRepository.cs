using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Features;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure.Mapping;
using TaskService.Infrastructure.Persistence.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TaskDbContext _dbContext;

    public CategoryRepository(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        category.Name = category.Name.ToLower();
        await _dbContext.Categories.AddAsync(category.ToDb(), cancellationToken);
        return category.Id;
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var categoryDb = await _dbContext.Categories.FindAsync(id, cancellationToken);
        if (categoryDb == null)
        {
            throw new InvalidOperationException("There is no category with such id");
        }

        return categoryDb.ToDomain();
    }

    public async Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidOperationException("The name cannot be empty");
        }

        name = name.ToLower();
        var categoryDb = await _dbContext.Categories.FirstOrDefaultAsync(
            c => c.Name == name, cancellationToken);
        if (categoryDb == null)
        {
            throw new InvalidOperationException("There is no category with such name");
        }

        return categoryDb.ToDomain();
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        var categoryDb = await _dbContext.Categories.FindAsync(category.Id, cancellationToken);
        if (categoryDb == null)
        {
            throw new InvalidOperationException("There is no category with such Id");
        }

        categoryDb.Name = category.Name.ToLower();
        categoryDb.Description = category.Description;
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken)
    {
        var categoryDb = await _dbContext.Categories.FindAsync(id, cancellationToken);
        if (categoryDb == null)
        {
            throw new InvalidOperationException("There is no category with such Id");
        }

        _dbContext.Categories.Remove(categoryDb);
    }
}