namespace AuthService.Core.ValueObjects;

public class FullName
{
    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Constructor
    /// </summary>
    public FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}