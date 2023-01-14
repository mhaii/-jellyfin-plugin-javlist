using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AvList.Entities;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AvList.Providers.JavList;

public class JavListImageProvider : IRemoteImageProvider
{
    private readonly ILogger<JavListImageProvider> _logger;
    private readonly JavListApi _javListApi;

    public JavListImageProvider(
        ILogger<JavListImageProvider> logger,
        JavListApi javListApi)
    {
        _logger = logger;
        _javListApi = javListApi;
    }

    /// <inheritdoc />
    public string Name => "JavList";

    /// <inheritdoc />
    public bool Supports(BaseItem item) => item is Jav;

    /// <inheritdoc />
    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return new[] { ImageType.Primary, ImageType.Banner };
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
    {
        var list = new List<RemoteImageInfo>();

        var javId = item.GetProviderId(ProviderNames.JavList);

        if (string.IsNullOrEmpty(javId))
        {
            return list;
        }

        var jav = await _javListApi.GetVideo(javId, cancellationToken).ConfigureAwait(false);
        if (jav == null)
        {
            return list;
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (jav.CoverUrl != null)
        {
            list.Add(new RemoteImageInfo { ProviderName = Name, Type = ImageType.Primary, Url = jav.CoverUrl });
        }

        return list;
    }

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }
}
