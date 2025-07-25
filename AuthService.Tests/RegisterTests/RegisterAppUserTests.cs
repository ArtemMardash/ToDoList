using AuthService.Features.Authentication.Register.Models;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence;
using FluentAssertions;
using Mediator;

namespace AuthService.Tests.RegisterTests;

public class RegisterAppUserTests: IDisposable
{
    private IAppUserRepository _appUserRepository;
    private AuthDbContext _dbContext;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private IMediator _mediator;

    public RegisterAppUserTests()
    {
        _appUserRepository = _dbService.GetRequiredService<IAppUserRepository>();
        _dbContext = _dbService.GetRequiredService<AuthDbContext>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _dbService.Migrate(_dbContext);
    }

    [Fact]
    public async Task Register_Handler_Should_Be_Successful()
    {
        var registerRequest = new RegisterRequest
        {
            Email = "ar2004@gmail.com",
            Password = "Ar12345!",
            FirstName = "Ar",
            LastName = "Tem"
        };

        var result = await _mediator.Send(registerRequest, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("emailgmai", "Art12345!123", "Artem", "Mardakhaev")]
    [InlineData("email@gmail.com", "Art12", "artem", "Mardakhaev")]
    public async Task Register_Handler_Should_Fail(string email, string password, string fisrtName, string lastName)
    {
        var registerRequest = new RegisterRequest
        {
            Email = email,
            Password = password,
            FirstName = fisrtName,
            LastName = lastName
        };

        var test = async () => await _mediator.Send(registerRequest, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}