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
    private IAppUserRepository _appUserRepository;
    private AuthDbContext _dbContext;
    private Guid _existedAppUserId;

    public PersistenceAppUserTests()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;
        var loggerMock = Substitute.For<ILogger<UserManager<AppUserDb>>>();


        services.AddDbContext<AuthDbContext>(opt =>
        {
            opt.UseNpgsql(
                connectionString:
                $"Host=localHost;Port=5432;Database=TestAuthDb_{Guid.NewGuid()};Username=postgres;Password=postgres",
                builder => builder.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name));
        });
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddIdentity<AppUserDb, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<ILogger<UserManager<AppUserDb>>>(sp => loggerMock);

        var scope = builder.Build().Services.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        _appUserRepository = serviceProvider.GetRequiredService<IAppUserRepository>();
        _dbContext = serviceProvider.GetRequiredService<AuthDbContext>();
        _dbContext.Database.Migrate();

         _existedAppUserId = _appUserRepository.CreateUserAsync(
            new AppUser("email@gmail.com", "Ar12345!", new FullName("Maxim", "Kyznechick")), CancellationToken.None)
             .GetAwaiter()
             .GetResult();
         
        _dbContext.SaveChanges();
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
}