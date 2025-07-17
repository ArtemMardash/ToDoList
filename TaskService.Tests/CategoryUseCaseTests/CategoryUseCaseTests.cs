using FluentAssertions;
using Mediator;
using TaskService.Features.Categories.AddCategory.Models;
using TaskService.Features.Categories.DeleteCategory.Models;
using TaskService.Features.Categories.GetCategoryById.Models;
using TaskService.Features.Categories.GetCategoryByName.Models;
using TaskService.Features.Categories.UpdateCategory.Models;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure;

namespace TaskService.Tests.CategoryUseCaseTests;

public class CategoryUseCaseTests: IDisposable
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly TaskDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private static Guid _categoryId;

    public CategoryUseCaseTests()
    {
        _categoryRepository = _dbService.GetRequiredService<ICategoryRepository>();
        _dbContext = _dbService.GetRequiredService<TaskDbContext>();
        _dbService.Migrate(_dbContext);
        _mediator = _dbService.GetRequiredService<IMediator>();
        _categoryId = _dbService.GetCategoryId(_categoryRepository, _dbContext);
    }

    [Fact]
    public async Task Create_Category_Should_Success_UseCase_Async()
    {
        var request = new AddCategoryRequest
        {
            Name = "New category for use case",
            Description = "Description of new category"
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "asdsdcsdacsdcsd")]
    [InlineData("ascsdcsdcsd", "")]
    [InlineData("", "")]
    [InlineData("NewCategory", "Descrption of new category")]
    public async Task Create_Category_Should_Success_UseCase_Fail(string name, string description)
    {
        var request = new AddCategoryRequest
        {
            Name = name,
            Description = description
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Delete_Category_Should_Success()
    {
        var request = new DeleteCategoryRequest
        {
            CategoryId = _categoryId
        };

        await _mediator.Send(request, CancellationToken.None);
        _dbContext.Categories.FirstOrDefault(c => c.Id == _categoryId).Should().BeNull();
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Delete_Category_Should_Fail(string Id)
    {
        var request = new DeleteCategoryRequest
        {
            CategoryId = Guid.Parse(Id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_Category_By_Id_Should_Success()
    {
        var request = new GetCategoryByIdRequest()
        {
            Id = _categoryId
        };

        var result = await _mediator.Send(request, CancellationToken.None);
        await Verify(result);
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    public async Task Get_Category_By_Id_Should_Fail(string Id)
    {
        var request = new GetCategoryByIdRequest()
        {
            Id = Guid.Parse(Id)
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Get_Category_By_Name_Should_Success()
    {
        var request = new GetCategoryByNameRequest
        {
            Name = "NewCategory"
        };

        var result = await _mediator.Send(request, CancellationToken.None);
        await Verify(result);
    }

    [Theory]
    [InlineData("adda6bea-f80a-448e-92e4-d02ec95bf778")]
    [InlineData("")]
    public async Task Get_Category_By_Name_Should_Fail(string name)
    {
        var request = new GetCategoryByNameRequest
        {
            Name = name
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Update_Category_Should_Success_UseCase_Async()
    {
        var request = new UpdateCategoryRequest
        {
            Id = _categoryId,
            Name = "new name",
            Description = "new description"
        };

        await _mediator.Send(request, CancellationToken.None);

        var result = _dbContext.Categories.FirstOrDefault(c => c.Id == _categoryId);
        result!.Name.Should().Be("new name");
        result.Description.Should().Be("new description");
    }

    [Theory]
    [InlineData("", "asdsdcsdacsdcsd")]
    [InlineData("ascsdcsdcsd", "")]
    [InlineData("", "")]
    public async Task Update_Category_Should_UseCase_Fail(string name, string description)
    {
        var request = new UpdateCategoryRequest()
        {
            Id = _categoryId,
            Name = name,
            Description = description
        };

        var test = async () => await _mediator.Send(request, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}