using System.Text;
using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Jwt;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Entities;
using AuthService.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace AuthService.Tests;

public class DbForTests
{
    private IServiceProvider _serviceProvider;


    public DbForTests()
    {
        var builder = WebApplication.CreateBuilder();
        var configuration = GetConfiguration();
        var services = builder.Services;
        var loggerMock = Substitute.For<ILogger<UserManager<AppUserDb>>>();
        
        var jwtSettings = configuration.GetSection("Jwt")?? throw new InvalidOperationException("No Jwt section");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);


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
        
        services.AddScoped<JwtService>();
        services.Configure<JwtSettings>(jwtSettings);

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"]
                };
            });
        
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Transient;
        });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        var scope = builder.Build().Services.CreateScope();

        _serviceProvider = scope.ServiceProvider;
    }

    private IConfiguration GetConfiguration()
    {
        var fileName = "appsettings.json";
        var config = new ConfigurationBuilder()
            .AddJsonFile(fileName)
            .AddEnvironmentVariables()
            .Build();
        return config;
    }

    public T GetRequiredService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
    public Guid GetUserId(IAppUserRepository userRepository, DbContext context)
    {
        var id = userRepository.CreateUserAsync(
                new AppUser("email@gmail.com", "Ar12345!", new FullName("Maxim", "Kyznechick")), CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        context.SaveChanges();
        return id;
    }

    public void Migrate(DbContext context)
    {
        context.Database.Migrate();
    }
    
}