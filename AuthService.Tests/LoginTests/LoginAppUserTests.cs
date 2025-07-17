using AuthService.Features.Authentication.Login.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence;
using FluentAssertions;
using Mediator;

namespace AuthService.Tests.LoginTests;

public class LoginAppUserTests: IDisposable
{
    private IAppUserRepository _appUserRepository;
    private AuthDbContext _dbContext;
    private Guid _existedAppUserId;
    private readonly IntegrationTestsHelper _dbService = new IntegrationTestsHelper();
    private IMediator _mediator;
    private IJwtService _jwtService;

    public LoginAppUserTests()
    {
        _appUserRepository = _dbService.GetRequiredService<IAppUserRepository>();
        _dbContext = _dbService.GetRequiredService<AuthDbContext>();
        _dbService.Migrate(_dbContext);
        _mediator = _dbService.GetRequiredService<IMediator>();
        _jwtService = _dbService.GetRequiredService<IJwtService>();
        _existedAppUserId = _dbService.GetUserId(_appUserRepository, _dbContext);
    }

    [Fact]
    public async Task Login_Handler_Should_Be_Successful()
    {
        var loginRequest = new LoginRequest
        {
            Email = "email@gmail.com",
            Password = "Ar12345!"
        };

        var result = await _mediator.Send(loginRequest, CancellationToken.None);


        result.AccessToken.Should().NotBeNull();
        result.RefreshToken.Should().NotBeNull();
        _jwtService.ValidateToken(result.AccessToken, false).Should().NotBeNull();
        _jwtService.ValidateToken(result.RefreshToken, true).Should().NotBeNull();
    }

    [Theory]
    [InlineData("email@gmail.com", "Art12345!123", "Wrong Password")]
    [InlineData("email@gmail.co", "Art12345!", "There is no appUser with such email")]
    public async Task Login_Handler_Should_Fail(string email, string password, string exceptionMessage)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var test = async () => await _mediator.Send(loginRequest, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}