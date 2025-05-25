using AuthService.Features.Authentication.Login.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.RefreshToken.Models;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence;
using FluentAssertions;
using Mediator;

namespace AuthService.Tests.RefreshTokenTests;

public class RefreshTokenAppUserTests
{
    private IAppUserRepository _appUserRepository;
    private AuthDbContext _dbContext;
    private readonly IntegrationTestsHelper _dbService = new();
    private IMediator _mediator;
    private Guid _existedAppUserId;
    private IJwtService _jwtService;

    public RefreshTokenAppUserTests()
    {
        _appUserRepository = _dbService.GetRequiredService<IAppUserRepository>();
        _dbContext = _dbService.GetRequiredService<AuthDbContext>();
        _dbService.Migrate(_dbContext);
        _mediator = _dbService.GetRequiredService<IMediator>();
        _existedAppUserId = _dbService.GetUserId(_appUserRepository, _dbContext);
        _jwtService = _dbService.GetRequiredService<IJwtService>();
    }

    [Fact]
    public async Task Refresh_User_Token_Should_Success()
    {
        var loginRequest = new LoginRequest
        {
            Email = "email@gmail.com",
            Password = "Ar12345!"
        };

        var tokens =await _mediator.Send(loginRequest, CancellationToken.None);

        var refreshTokenRequest = new RefreshTokenRequest
        {
            RefreshToken = tokens.RefreshToken
        };

        var result = await _mediator.Send(refreshTokenRequest, CancellationToken.None);

        result.RefreshToken.Should().NotBe(tokens.RefreshToken);
        result.AccessToken.Should().NotBe(tokens.AccessToken);
        _jwtService.ValidateToken(result.RefreshToken, true).Should().NotBeNull();
        _jwtService.ValidateToken(result.AccessToken, false).Should().NotBeNull();
    }
    
    
    

}