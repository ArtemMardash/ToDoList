using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Entities;
using AuthService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ILogger = Castle.Core.Logging.ILogger;

namespace AuthService.Tests.PersistenceTests;

public class PersistenceAppUserTests: IDisposable
{
    private static IAppUserRepository _appUserRepository;
    private static AuthDbContext _dbContext;
    private static Guid _existedAppUserId;
    private readonly DbForTests _dbService = new DbForTests();

    public PersistenceAppUserTests()
    {
        _appUserRepository = _dbService.GetRequiredService<IAppUserRepository>();
        _dbContext = _dbService.GetRequiredService<AuthDbContext>();
        _dbService.Migrate(_dbContext);
        _existedAppUserId = _dbService.GetUserId(_appUserRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Create_App_User_Should_Successful()
    {
        var appUser = new AppUser(email: "mart@gmail.com", password: "Aa123456!",
            new FullName(firstName: "artem", lastName: "mardakhaev"));
        var id = await _appUserRepository.CreateUserAsync(appUser, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var appUserDb = await _appUserRepository.GetUserByIdAsync(id, CancellationToken.None);
        appUserDb.Should().NotBeNull();
        appUserDb.Id.Should().Be(appUser.Id);
        appUserDb.FullName.ToString().Should().Be(appUser.FullName.ToString());
        appUserDb.Email.Should().Be(appUser.Email);
    }

    [Fact]
    public async Task Get_User_By_Id_Should_Be_Successful()
    {
        var appUser = await _appUserRepository.GetUserByIdAsync(_existedAppUserId, CancellationToken.None);

        var settings = new VerifySettings();
        settings.IgnoreMember(nameof(appUser.Password));
        await Verify(appUser, settings);
    }
    
    [Fact]
    public async Task Get_User_By_Email_Should_Be_Successful()
    {
        var appUser = await _appUserRepository.GetUserByEmail("email@gmail.com", CancellationToken.None);

        var settings = new VerifySettings();
        settings.IgnoreMember(nameof(appUser.Password));
        await Verify(appUser, settings);
    }
}