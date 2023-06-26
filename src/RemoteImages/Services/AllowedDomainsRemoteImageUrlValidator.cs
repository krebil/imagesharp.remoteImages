using Microsoft.Extensions.Options;
using RemoteImages.Options;

namespace RemoteImages.Services;

public class AllowedDomainsRemoteImageUrlValidator : IRemoteImageUrlValidator
{
    private readonly List<string> _allowedDomains;

    public AllowedDomainsRemoteImageUrlValidator(IOptions<AllowedDomainsRemoteImageUrlValidatorOptions> options)
    {
        _allowedDomains = options.Value.AllowedDomains
            .Select(domain => domain.ToLower())
            .ToList();
    }

    public bool IsValidUrl(Uri url)
    {
        return _allowedDomains.Contains(url.Host);
    }
}