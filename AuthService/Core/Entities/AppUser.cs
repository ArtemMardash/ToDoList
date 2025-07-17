using AuthService.Core.ValueObjects;
using AuthService.Infrastructure.Extensions;

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
        Email = email.IsValidEmail() ? email : throw new InvalidOperationException("Invalid email");
        SetPassword(password);
        FullName = fullName;
    }

    /// <summary>
    /// Constructor for creation
    /// </summary>
    public AppUser(string email, string password, FullName fullName)
    {
        Id = Guid.NewGuid();
        Email = email.IsValidEmail() ? email : throw new InvalidOperationException("Invalid email");
        Password = password;
        FullName = fullName;
    }

    public void SetPassword(string pass)
    {
        if (string.IsNullOrWhiteSpace(pass))
        {
            throw new InvalidOperationException("The password can not be empty");
        }
        else
        {
            Password = pass;
        }
    }
}