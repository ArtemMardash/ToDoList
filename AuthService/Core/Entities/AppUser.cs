using AuthService.Core.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Core.Entities;

public class AppUser
{
    public Guid Id { get; private set; }
    
    public string Email { get; private set; }
    
    public string Password { get; private set; }
    
    public FullName FullName { get; private set; }
    

    public AppUser(Guid id, string email, string password, FullName fullName)
    {
        Id = id;
        Email = email;
        Password = password;
        FullName = fullName;
    }

    public AppUser(string email, string password, FullName fullName)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        FullName = fullName;
    }
    
}