namespace SharedKernel;

public interface IGoogleLogin
{
    public Guid UserId { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }

    public DateTime TokenExpiry { get; set; }
}