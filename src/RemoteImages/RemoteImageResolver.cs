using System.Net;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Resolvers;

namespace RemoteImages;

public class RemoteImageResolver: IImageResolver
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string? _url;

    public const string HttpClientName = "RemoteImageResolver";

    public RemoteImageResolver(IHttpClientFactory clientFactory, string? url)
    {
        _clientFactory = clientFactory;
        _url = url;
    }

    public async Task<ImageMetadata> GetMetaDataAsync()
    {
        var client = _clientFactory.CreateClient(HttpClientName);

        //First try to get the headers only
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _url), HttpCompletionOption.ResponseHeadersRead);

        if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
        {
            //If endpoint does not allow headers only, get the full response
            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _url), HttpCompletionOption.ResponseHeadersRead);
        }

        if (!response.Content.Headers.ContentLength.HasValue)
        {
            throw new Exception("Required header ContentLength is missing.");
        }

        if (!response.Content.Headers.LastModified.HasValue || !response.Content.Headers.ContentLength.HasValue)
        {
            return new ImageMetadata(new DateTime(), response.Content.Headers.ContentLength.Value);
        }

        return new ImageMetadata(response.Content.Headers.LastModified.Value.UtcDateTime, response.Content.Headers.ContentLength.Value);
    }

    public async Task<Stream> OpenReadAsync()
    {
        var client = _clientFactory.CreateClient(HttpClientName);

        var response = await client.GetAsync(_url);

        return await response.Content.ReadAsStreamAsync();
    }
}