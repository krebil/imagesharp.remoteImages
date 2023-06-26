using SixLabors.ImageSharp.Web.DependencyInjection;

namespace RemoteImages.Extensions;

public static class ImageSharpBuilderExtensions
{
    public static IImageSharpBuilder AddImageSharpRemoteImageProvider(this IImageSharpBuilder builder)
    {
        return builder.AddProvider<RemoteImageProvider>();
    }
}