using System.Text;
using AuthService.Features.Authentication.Login.Services;
using AuthService.Features.Authentication.Shared.Interfaces;
using AuthService.Features.Authentication.Shared.Repositories;
using AuthService.Features.Authentication.Shared.Settings;
using AuthService.Infrastructure.Jwt;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Entities;
using AuthService.Infrastructure.Persistence.Repositories;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSection = configuration.GetSection("RabbitMq") ??
                              throw new InvalidOperationException("There is no section RabbitMq");
        var rabbitHost = rabbitMqSection["Host"] ??
                         throw new InvalidOperationException("There is no rabbit host");
        var login = rabbitMqSection["Login"] ??
                    throw new InvalidOperationException("There is no login in rabbit mq");
        var password = rabbitMqSection["Password"] ??
                       throw new InvalidOperationException("There is no password in rabbit mq");
        services.AddScoped<IBrokerPublisher, BrokerPublisher>();    
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                
                cfg.Host($"rabbitmq://{rabbitHost}", host =>
                {
                    host.Username(login);
                    host.Password(password);
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        return services;
    }
    
    

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt") ?? throw new InvalidOperationException("No Jwt section");
        var externalAuthSettings = configuration.GetSection(nameof(ExternalAuthSettings)) ??
                                   throw new InvalidOperationException("No external auth section");
        var key = Encoding.ASCII.GetBytes(jwtSettings["AccessToken:Key"]!);
        services.AddScoped<IJwtService, JwtService>();
        services.Configure<JwtSettings>(jwtSettings);

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Bearer", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
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
            })
            .AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.Lax;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(opt =>
            {
                opt.ClientId = externalAuthSettings["Google:client_id"];
                opt.ClientSecret = externalAuthSettings["Google:client_secret"];
                opt.CallbackPath = "/api/auth/google/callback";

                opt.AccessType = "offline";
                opt.SaveTokens  = true;

                opt.Scope.Add("https://www.googleapis.com/auth/calendar");
                opt.Scope.Add("https://www.googleapis.com/auth/calendar.events");
            });
    }
}