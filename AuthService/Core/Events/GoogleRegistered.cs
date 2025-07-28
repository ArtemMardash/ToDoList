using AuthService.Core.ValueObjects;

namespace AuthService.Core.Events;

public class GoogleRegistered
{
    
    /// <summary>
    /// Id of Application User
    /// </summary>
    public Guid Id { get; set; }
    
    public string GoogleId { get; set; }
    
    public string GoogleRefreshToken { get; set; }
    
    public string GoogleAccessToken { get; set; }
    
    public DateTime TokenExpiry { get; set; }
}