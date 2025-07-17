using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Repositories;

namespace TaskService.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISubTaskRepository, SubTaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<TaskDbContext>(opt =>
        {
            opt.UseNpgsql(
                configuration.GetConnectionString("DefaultConnectionString") ??
                throw new InvalidOperationException("Invalid Default Connection String"),
                builder => builder.MigrationsAssembly(typeof(TaskDbContext).Assembly.GetName().Name));
        });
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
}