using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TaskService.Core.Entities;
using TaskService.Core.Enums;
using TaskService.Features.Shared.Interfaces;
using TaskService.Features.Shared.Repositories;
using TaskService.Infrastructure;
using TaskService.Infrastructure.Extensions;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Persistence.Entities;
using TaskService.Infrastructure.Repositories;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace TaskService.Tests;

public class IntegrationTestsHelper
{
    private IServiceProvider _serviceProvider;


    public IntegrationTestsHelper()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;


        services.AddDbContext<TaskDbContext>(opt =>
        {
            opt.UseNpgsql(
                connectionString:
                $"Host=localHost;Port=5432;Database=TestTaskDb_{Guid.NewGuid()};Username=postgres;Password=postgres",
                builder => builder.MigrationsAssembly(typeof(TaskDbContext).Assembly.GetName().Name));
        });

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISubTaskRepository, SubTaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Transient; });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        var scope = builder.Build().Services.CreateScope();

        _serviceProvider = scope.ServiceProvider;
    }


    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public Guid CreateTask(ITaskRepository taskRepository, DbContext context)
    {
        var id = taskRepository.CreateTaskAsync(
                new ToDoTask(
                    Guid.Parse("b920aec4-3e93-4172-a4df-97f805c86ffb"),
                    Guid.Parse("b57d3d21-1b25-49d8-92c0-275034d35942"),
                    "PushUps",
                    "Do 20 push ups",
                    new Category("Sport", "Something involved exercising"),
                    DateTime.UtcNow.AddDays(7),
                    TaskAndSubtaskStatus.New,
                    new List<Subtask>()
                ), CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        context.SaveChanges();
        return id;
    }

    public Guid GetSubTaskId(ISubTaskRepository subTaskRepository, ITaskRepository taskRepository, Guid taskId,
        DbContext context)
    {
        var parentTask = taskRepository.GetTaskByIdAsync(taskId, CancellationToken.None).GetAwaiter().GetResult();
        var id = subTaskRepository.CreateSubTaskAsync(new Subtask(
                "SubTaskName",
                TaskAndSubtaskStatus.New,
                parentTask), CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        context.SaveChanges();
        return id;
    }

    public Guid GetCategoryId(ICategoryRepository categoryRepository, DbContext context)
    {
        var id = categoryRepository
            .CreateCategoryAsync(
                new Category("NewCategory", "Descrption of new category"),
                CancellationToken.None)
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