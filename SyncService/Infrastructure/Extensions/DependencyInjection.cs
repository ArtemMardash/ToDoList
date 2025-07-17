using MassTransit;
using Microsoft.EntityFrameworkCore;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.BackgroundJobs.UseCases;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.GoogleAccessConsumers;
using SyncService.Infrastructure.Persistence;
using SyncService.Infrastructure.Persistence.Repositories;
using SyncService.Infrastructure.TaskConsumers;

namespace SyncService.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITaskSyncMappingRepository, TaskSyncMappingRepository>();
        services.AddScoped<IUserSyncStateRepository, UserSyncStateRepository>();
        services.AddDbContext<SyncDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("DefaultConnectionString") ??
                          throw new InvalidOperationException("Invalid Default Connection String"),
                builder => builder.MigrationsAssembly(typeof(SyncDbContext).Assembly.GetName().Name));
        });
        return services;
    }

    public static IServiceCollection RegisterBackgroundJob(this IServiceCollection services)
    {
        services.AddScoped<IGoogleLoginUseCase, GoogleLoginUseCase>();
        services.AddScoped<IGoogleRegisterUseCase, GoogleRegistredUseCase>();
        services.AddScoped<ITaskCreatedUseCase, TaskCreatedUseCase>();
        services.AddScoped<ITaskDeletedUseCase, TaskDeletedUseCase>();
        services.AddScoped<ITaskUpdatedUseCase, TaskUpdatedUseCase>();
        return services;
    }

    public static IServiceCollection AddQueue(this IServiceCollection services, IConfiguration config)
    {
        var rabbitMqSection = config.GetSection("RabbitMq") ??
                              throw new InvalidOperationException("There is no section RabbitMq");
        var rabbitHost = rabbitMqSection["Host"] ??
                         throw new InvalidOperationException("There is no rabbit host");
        var login = rabbitMqSection["Login"] ??
                    throw new InvalidOperationException("There is no login in rabbit mq");
        var password = rabbitMqSection["Password"] ??
                       throw new InvalidOperationException("There is no password in rabbit mq");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TaskCreatedConsumer>();
            x.AddConsumer<TaskUpdatedConsumer>();
            x.AddConsumer<TaskDeletedConsumer>();
            x.AddConsumer<LoginConsumer>();
            x.AddConsumer<RegisteredConsumer>();
            x.UsingRabbitMq((ctx,cfg) =>
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