using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SyncService.Core.Entities;
using SyncService.Infrastructure.Persistence.DbEntities;

namespace SyncService.Infrastructure.Persistence;

public class SyncDbContext: DbContext
{
    public DbSet<TaskSyncMappingDb> TasksSyncMapping { get; set; }
    
    public DbSet<UserSyncStateDb> UsersSyncState { get; set; }
    
    public SyncDbContext(DbContextOptions options): base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskSyncMappingDb>().HasKey(t => t.Id);
        modelBuilder.Entity<UserSyncStateDb>().HasKey(u => u.Id);
    } 
}

public class UserContextFactory : IDesignTimeDbContextFactory<SyncDbContext>
{
    public SyncDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SyncDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Sync;Username=postgres;Password=postgres",
            builder => builder.MigrationsAssembly(typeof(SyncDbContext).Assembly.GetName().Name));

        return new SyncDbContext(optionsBuilder.Options);
    }
}