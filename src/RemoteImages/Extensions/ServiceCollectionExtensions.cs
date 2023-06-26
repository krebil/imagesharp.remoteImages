using Microsoft.Extensions.DependencyInjection;
using RemoteImages.Options;
using RemoteImages.Services;

namespace RemoteImages.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddImageSharpRemoteImageDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<AllowedDomainsRemoteImageUrlValidatorOptions>().BindConfiguration(AllowedDomainsRemoteImageUrlValidatorOptions.AllowedDomainsRemoteImageUrlValidator);
        serviceCollection.AddTransient<IRemoteImageUrlValidator, AllowedDomainsRemoteImageUrlValidator>();
    }
}