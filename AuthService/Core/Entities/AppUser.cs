using AuthService.Core.ValueObjects;

namespace AuthService.Core.Entities;

public class AppUser
{
    /// <summary>
    /// Id of Application User
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Email of user
    /// </summary>
    public string Email { get; private set; }
    
    /// <summary>
    /// Password of user
    /// </summary>
    public string Password { get; private set; }
    
    /// <summary>
    /// Full Name of User
    /// </summary>
    public FullName FullName { get; private set; }
    

    /// <summary>
    /// Constructor for parsing
    /// </summary>
    public AppUser(Guid id, string email, string password, FullName fullName)
    {
        Id = id;
        Email = email;
        Password = password;
        FullName = fullName;
    }

    /// <summary>
    /// Constructor for creation
    /// </summary>
    public AppUser(string email, string password, FullName fullName)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        FullName = fullName;
    }
    
}