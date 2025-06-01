using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AuthService.Features.Authentication.Shared.Settings;

public class ExternalAuthSettings
{
    [JsonPropertyName("Google")] public GoogleAuthSettings Google { get; set; }
}

public class GoogleAuthSettings
{
    [JsonPropertyName("client_id")] public string ClientId { get; set; }

    [JsonPropertyName("project_id")] public string ProjectId { get; set; }

    [JsonPropertyName("auth_uri")] public string AuthUri { get; set; }

    [JsonPropertyName("token_uri")] public string TokenUri { get; set; }

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderx509CertUrl { get; set; }

    [JsonPropertyName("client_secret")] public string ClientSecret { get; set; }

    [JsonPropertyName("redirect_uris")] public List<string> RedirestUris { get; set; }
}