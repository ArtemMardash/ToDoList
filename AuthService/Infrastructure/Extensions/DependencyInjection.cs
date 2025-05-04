using System.Text;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Infrastructure.Jwt;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Entities;
using AuthService.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        
        services.AddDbContext<AuthDbContext>(opt =>
        {
            opt.UseNpgsql(
                configuration.GetConnectionString("DefaultConnectionString") ??
                throw new InvalidOperationException("Invalid Default Connection String"),
                builder => builder.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name));
        });
        services.AddIdentity<AppUserDb, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Transient;
        });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt")?? throw new InvalidOperationException("No Jwt section");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);
        services.AddScoped<IJwtService, JwtService>();
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
    }
}