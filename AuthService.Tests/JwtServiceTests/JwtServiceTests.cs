using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Tests.JwtServiceTests;

public class JwtServiceTests
{
    private readonly IntegrationTestsHelper _integrationTestsHelper = new IntegrationTestsHelper();
    private IAppUserRepository _repository;
    private IJwtService _jwtService;
    private Guid _existedAppUserId;
    private DbContext _dbContext;

    public JwtServiceTests()
    {
        _repository = _integrationTestsHelper.GetRequiredService<IAppUserRepository>();
        _jwtService = _integrationTestsHelper.GetRequiredService<IJwtService>();
        _dbContext = _integrationTestsHelper.GetRequiredService<AuthDbContext>();
        _integrationTestsHelper.Migrate(_dbContext);
        _existedAppUserId = _integrationTestsHelper.GetUserId(_repository, _dbContext);
    }


    [Fact]
    public async Task Generate_Access_Token_Should_Success()
    {
        var user = await _repository.GetUserByIdAsync(_existedAppUserId, CancellationToken.None);
        var token = _jwtService.GenerateAccessToken(user);

        var claims = _jwtService.ValidateToken(token, false);

        Verify(claims);
    }

    [Fact]
    public async Task Generate_Refresh_Token_Should_Success()
    {
        var user = await _repository.GetUserByIdAsync(_existedAppUserId, CancellationToken.None);
        var token = _jwtService.GenerateRefreshToken(user);

        var claims = _jwtService.ValidateToken(token.refreshToken, true);

        Verify(claims);
    }
}