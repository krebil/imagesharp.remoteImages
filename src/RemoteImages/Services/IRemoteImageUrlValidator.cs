namespace RemoteImages.Services;

public interface IRemoteImageUrlValidator
{
    bool IsValidUrl(Uri url);
}