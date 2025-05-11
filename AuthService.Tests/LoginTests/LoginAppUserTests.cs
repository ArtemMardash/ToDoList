using System.ComponentModel.DataAnnotations;
using AuthService.Features.Authentication.Login.Models;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Login.UseCases;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Jwt;
using AuthService.Infrastructure.Persistence;
using FluentAssertions;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Options;

namespace AuthService.Tests.LoginTests;

public class LoginAppUserTests
{
    private IAppUserRepository _appUserRepository;
    private AuthDbContext _dbContext;
    private Guid _existedAppUserId;
    private readonly DbForTests _dbService = new DbForTests();
    private IMediator _mediator;

    public LoginAppUserTests()
    {
        _appUserRepository = _dbService.GetRequiredService<IAppUserRepository>();
        _dbContext = _dbService.GetRequiredService<AuthDbContext>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _dbService.Migrate(_dbContext);
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

        var result =await _mediator.Send(loginRequest, CancellationToken.None);

        result.AccessToken.Should().NotBeNull();
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

        var test = async ()=> await _mediator.Send(loginRequest, CancellationToken.None);

        await test.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
    }
}