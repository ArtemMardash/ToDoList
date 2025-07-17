namespace AuthService.Core.ValueObjects;

public class FullName
{
    private const byte FIRST_NAME_MAX = 30;

    private const byte LAST_NAME_MAX = 50;
    
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
        SetFirstName(firstName);
        SetLastName(lastName);
    }
    
    /// <summary>
    /// Method to set a first name
    /// </summary>
    private void SetFirstName(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > FIRST_NAME_MAX)
        {
            throw new ArgumentOutOfRangeException(nameof(FirstName), "First name is too long or empty");
        }
        else
        {
            input = input.ToLower();
            FirstName = $"{char.ToUpper(input[0])}{input.Substring(1)}";
        }
    }

    /// <summary>
    /// Method to setl a last name
    /// </summary>
    private void SetLastName(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > LAST_NAME_MAX)
        {
            throw new ArgumentOutOfRangeException(nameof(LastName), "Last name is too long or empty");
        }
        else
        {
            input = input.ToLower();
            LastName = $"{char.ToUpper(input[0])}{input.Substring(1)}";
        }
    }


    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}