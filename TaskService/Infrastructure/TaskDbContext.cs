using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TaskService.Infrastructure.Persistence.Entities;

namespace TaskService.Infrastructure;

public class TaskDbContext: DbContext
{
    /// <summary>
    /// Database for tasks
    /// </summary>
    public DbSet<TaskDb> Tasks { get; set; }
    
    /// <summary>
    /// Database for categpries
    /// </summary>
    public DbSet<CategoryDb> Categories { get; set; }
    
    /// <summary>
    /// Database for subtasks
    /// </summary>
    public DbSet<SubTaskDb> Subtasks { get; set; }

    public TaskDbContext(DbContextOptions options): base(options)
    {
        //Database.EnsureCreated()
    }
 
    /// <summary>
    /// Data to db
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskDb>().HasKey(u => u.Id);
        modelBuilder.Entity<CategoryDb>().HasKey(u => u.Id);
        modelBuilder.Entity<SubTaskDb>().HasKey(u => u.Id);

        modelBuilder.Entity<TaskDb>().Property(u => u.Id).ValueGeneratedNever();
        modelBuilder.Entity<CategoryDb>().Property(u => u.Id).ValueGeneratedNever();
        modelBuilder.Entity<SubTaskDb>().Property(u => u.Id).ValueGeneratedNever();

        modelBuilder.Entity<TaskDb>().HasMany<SubTaskDb>(t => t.SubTasks).WithOne(s => s.Parent);
        modelBuilder.Entity<TaskDb>().HasOne<CategoryDb>(t => t.Category);
    }
}

public class UserContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Tasks;Username=postgres;Password=postgres",
            builder => builder.MigrationsAssembly(typeof(TaskDbContext).Assembly.GetName().Name));

        return new TaskDbContext(optionsBuilder.Options);
    }
}