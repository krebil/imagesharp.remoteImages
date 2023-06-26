namespace RemoteImages.Options;

public class AllowedDomainsRemoteImageUrlValidatorOptions
{
    public const string AllowedDomainsRemoteImageUrlValidator = "ImageSharp";
    public IEnumerable<string> AllowedDomains { get; set; } = Enumerable.Empty<string>();
    public string PathPrefix { get; set; } = string.Empty;
}