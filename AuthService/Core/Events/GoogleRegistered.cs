using AuthService.Core.ValueObjects;

namespace AuthService.Core.Events;

public class GoogleRegistered
{
    
    /// <summary>
    /// Id of Application User
    /// </summary>
    public Guid Id { get; set; }
    
    public string RefreshToken { get; set; }
    
    public string AccessToken { get; set; }
    
    public DateTime TokenExpiry { get; set; }
}