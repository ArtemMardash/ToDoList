using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Persistence.Entities;

public class AppUserDb: IdentityUser
{
    public string FullName { get; set; }
}