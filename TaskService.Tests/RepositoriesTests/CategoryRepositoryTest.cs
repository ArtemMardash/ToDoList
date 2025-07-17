using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure;

namespace TaskService.Tests.RepositoriesTests;

public class CategoryRepositoryTest : IDisposable
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly TaskDbContext _dbContext;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _categoryId;

    public CategoryRepositoryTest()
    {
        _categoryRepository = _dbService.GetRequiredService<ICategoryRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _categoryId = _dbService.GetCategoryId(_categoryRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task Create_Category_Should_Success_Async()
    {
        var category = new Category("Interesting category", "books");
        var id = await _categoryRepository.CreateCategoryAsync(category, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryByIdAsync = await _categoryRepository.GetCategoryByIdAsync(id, CancellationToken.None);
        categoryByIdAsync.Should().NotBeNull();
        categoryByIdAsync.Name.Should().Be(category.Name);
        categoryByIdAsync.Description.Should().Be(category.Description);
    }

    [Fact]
    public async Task Get_Category_By_Id_Should_Be_Successful()
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(_categoryId, CancellationToken.None);

        await Verify(category);
    }

    [Fact]
    public async Task Update_Category_Should_Be_Successful()
    {
        var categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(_categoryId, CancellationToken.None);
        categoryToUpdate.Description = "New description";
        categoryToUpdate.Name = "New super name";
        await _categoryRepository.UpdateCategoryAsync(categoryToUpdate, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryToCheck =
            await _dbContext.Categories
                .FirstOrDefaultAsync(ts => ts.Id == _categoryId, CancellationToken.None);
        await Verify(categoryToCheck);
    }


    [Fact]
    public async Task Delete_Category_Should_Be_Successful()
    {
        await _categoryRepository.DeleteCategoryAsync(_categoryId, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _dbContext.Categories.FirstOrDefault(st => st.Id == _categoryId).Should().BeNull();
    }
}