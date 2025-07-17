using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SyncService.BackgroundJobs.Interfaces;
using SyncService.BackgroundJobs.UseCases;
using SyncService.Core.Entities;
using SyncService.Features.Shared.Interfaces;
using SyncService.Features.Shared.Repositories;
using SyncService.Infrastructure.Extensions;
using SyncService.Infrastructure.Persistence;
using SyncService.Infrastructure.Persistence.Repositories;

namespace SyncService.Tests;

public class IntegrationTestsHelper
{
    private IServiceProvider _serviceProvider;


    public IntegrationTestsHelper()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITaskSyncMappingRepository, TaskSyncMappingRepository>();
        services.AddScoped<IUserSyncStateRepository, UserSyncStateRepository>();
        services.AddDbContext<SyncDbContext>(opt =>
        {
            opt.UseNpgsql(
                connectionString:
                $"Host=localHost;Port=5432;Database=TestSyncDb_{Guid.NewGuid()};Username=postgres;Password=postgres",
                builder => builder.MigrationsAssembly(typeof(SyncDbContext).Assembly.GetName().Name));
        });
        // services.AddScoped<IGoogleLoginUseCase, GoogleLoginUseCase>();
        // services.AddScoped<IGoogleRegisterUseCase, GoogleRegistredUseCase>();
        // services.AddScoped<ITaskCreatedUseCase, TaskCreatedUseCase>();
        // services.AddScoped<ITaskDeletedUseCase, TaskDeletedUseCase>();
        // services.AddScoped<ITaskUpdatedUseCase, TaskUpdatedUseCase>();

        var scope = builder.Build().Services.CreateScope();

        _serviceProvider = scope.ServiceProvider;
    }


    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public Guid CreateTask(ITaskSyncMappingRepository taskRepository, DbContext context)
    {
        var result =
            taskRepository
                .AddTaskSyncMappingAsync(new TaskSyncMapping(Guid.NewGuid(), "new caledar event"),
                    CancellationToken.None).GetAwaiter().GetResult();
        context.SaveChanges();
        return result;
    }

    public Guid CreateUser(IUserSyncStateRepository userRepository, DbContext context)
    {
        var result = userRepository
            .AddUserSyncStateAsync(
                new UserSyncState(
                    Guid.NewGuid(), 
                    "new google access token", 
                    "new google refresh token",
                    DateTime.UtcNow.AddDays(3)),
                CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        context.SaveChanges();
        return result;
    }


    public void Migrate(DbContext context)
    {
        context.Database.Migrate();
    }
}