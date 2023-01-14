using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AvList.Entities;
using Jellyfin.Plugin.AvList.Extensions;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using Pukenicorn.Sukebei.JavList.Client.Model;

namespace Jellyfin.Plugin.AvList.Providers.JavList;

public class JavListProvider : IRemoteMetadataProvider<Jav, JavInfo>, IHasOrder
{
    private readonly IApplicationPaths _paths;
    private readonly ILogger<JavListProvider> _logger;
    private readonly JavListApi _javListApi;

    public JavListProvider(
        IApplicationPaths appPaths,
        ILogger<JavListProvider> logger,
        JavListApi javListApi)
    {
        _paths = appPaths;
        _logger = logger;
        _javListApi = javListApi;
    }

    public int Order => -2;

    public string Name => "JavList";

    public async Task<MetadataResult<Jav>> GetMetadata(JavInfo info, CancellationToken cancellationToken)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        var result = new MetadataResult<Jav>();

        VideoDto? video;
        var code = info.ProviderIds.GetValueOrDefault(ProviderNames.JavList);
        if (!string.IsNullOrEmpty(code))
        {
            video = await _javListApi.GetVideo(code, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            _logger.LogInformation("Start JavList... Searching({Name})", info.Name);
            video = await _javListApi.GetVideo(info.Name, cancellationToken).ConfigureAwait(false);
            // MediaSearchResult msr = await _javListApi.Search_GetJav(info.Name, cancellationToken);
            // if (msr != null)
            // {
            //     media = await _javListApi.GetAnime(msr.id.ToString());
            // }
        }

        if (video == null)
        {
            return result;
        }

        result.HasMetadata = true;
        result.Item = video.ToJav();
        result.People = new List<PersonInfo>(video.GetCasts());
        result.Provider = ProviderNames.JavList;
        StoreImageUrl(video.Code, video.CoverUrl, "image");

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(JavInfo searchInfo, CancellationToken cancellationToken)
    {
        var results = new List<RemoteSearchResult>();

        var code = searchInfo.ProviderIds.GetValueOrDefault(ProviderNames.JavList);
        if (!string.IsNullOrEmpty(code))
        {
            var videoResult = await _javListApi.GetVideo(code, cancellationToken).ConfigureAwait(false);
            if (videoResult != null)
            {
                results.Add(videoResult.ToSearchResult());
            }
        }

        // if (string.IsNullOrEmpty(searchInfo.Name))
        // {
        //     return results;
        // }
        //
        // List<MediaSearchResult> name_results = await _javListApi.Search_GetJav_list(searchInfo.Name, cancellationToken).ConfigureAwait(false);
        // foreach (var media in name_results)
        // {
        //     results.Add(media.ToSearchResult());
        // }

        return results;
    }

    private void StoreImageUrl(string jav, string url, string type)
    {
        var path = Path.Combine(_paths.CachePath, "javList", type, jav + ".txt");
        var directory = Path.GetDirectoryName(path);
        Directory.CreateDirectory(directory);

        File.WriteAllText(path, url);
    }

    public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        var httpClient = Plugin.Instance!.GetHttpClient();

        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }
}
