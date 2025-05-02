using AuthService.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Persistence;

public class AuthDbContext: IdentityDbContext<AppUserDb>
{
    public AuthDbContext(DbContextOptions options): base(options)
    {
    }
    
}

public class UserContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AuthDb;Username=postgres;Password=postgres",
            builder => builder.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name));

        return new AuthDbContext(optionsBuilder.Options);
    }
}
