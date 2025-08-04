using System.Runtime.InteropServices.JavaScript;
using SharedKernel;

namespace SyncService.BackgroundJobs.Dtos;

public class TaskInfo
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime End { get; set; }
    
    public DateTime Start { get; set; }
}