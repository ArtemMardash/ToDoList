using SharedKernel;

namespace SyncService.BackgroundJobs.Dtos;

public class GoogleRegisterDto: IGoogleRegistered
{
    public Guid UserId { get; set; }
    
    public string GoogleId { get; set; }
    
    public string GoogleRefreshToken { get; set; }
    
    public string GoogleAccessToken { get; set; }
    
    public DateTime TokenExpiry { get; set; }
}