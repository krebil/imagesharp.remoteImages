using SixLabors.ImageSharp.Web.DependencyInjection;

namespace remoteImages.Extensions;

public static class ImageSharpBuilderExtensions
{
    public static IImageSharpBuilder AddImageSharpRemoteImageProvider(this IImageSharpBuilder builder)
    {
        return builder.AddProvider<RemoteImageProvider>();
    }
}