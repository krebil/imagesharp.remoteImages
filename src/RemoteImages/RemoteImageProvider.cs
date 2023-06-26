using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RemoteImages.Options;
using RemoteImages.Services;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace RemoteImages;

public class RemoteImageProvider : IImageProvider
{
    public ProcessingBehavior ProcessingBehavior => ProcessingBehavior.All;
    public Func<HttpContext, bool> Match { get; set; }


    private readonly IHttpClientFactory _clientFactory;
    private readonly IRemoteImageUrlValidator _urlValidator;
    private readonly IOptions<AllowedDomainsRemoteImageUrlValidatorOptions> _options;

    private static bool PathStartsWithRemote(HttpContext context,
        IOptions<AllowedDomainsRemoteImageUrlValidatorOptions> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.PathPrefix))
            return false;
        return context.Request.Path.StartsWithSegments(options.Value.PathPrefix, StringComparison.OrdinalIgnoreCase);
    }

    public RemoteImageProvider(IHttpClientFactory clientFactory, IRemoteImageUrlValidator urlValidator,
        IOptions<AllowedDomainsRemoteImageUrlValidatorOptions> options)
    {
        _clientFactory = clientFactory;
        _urlValidator = urlValidator;
        _options = options;

        Match = context => { return PathStartsWithRemote(context, _options) && IsValidRequest(context); };
    }

    public bool IsValidRequest(HttpContext context)
    {
        var url = GetRemoteUrl(context, _options.Value.PathPrefix);
        var isWellFormed = Uri.IsWellFormedUriString(url, UriKind.Absolute);
        if (!isWellFormed)
            return false;
        var isValidUrl = _urlValidator.IsValidUrl(new Uri(url!));
        return isValidUrl;
    }


    public async Task<IImageResolver?> GetAsync(HttpContext context)
    {
        var url = GetRemoteUrl(context, _options.Value.PathPrefix);
        return await Task.FromResult(new RemoteImageResolver(_clientFactory, url) as IImageResolver);
    }
    

    private static string? GetRemoteUrl(HttpContext context, string pathPrefix)
    {
        if (!context.Request.Path.HasValue || context.Request.Path.Value.Length <= pathPrefix.Length)
        {
            return null;
        }

        var remoteUrl = context.Request.Path.Value?.Substring(pathPrefix.Length + 1);
        return remoteUrl?.Replace(" ", "%20");
    }
}