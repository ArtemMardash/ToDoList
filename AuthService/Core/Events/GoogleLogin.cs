using Mediator;
using SharedKernel;

namespace AuthService.Core.Events;

public class GoogleLogin
{
    public Guid UserId { get; set; }
    
    public string GoogleId { get; set; }
    
    public string GoogleRefreshToken { get; set; }
    
    public string GoogleAccessToken { get; set; }
    
    public DateTime TokenExpiry { get; set; }
}