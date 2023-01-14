using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.AvList.Entities;
using Jellyfin.Plugin.AvList.Extensions;
using Microsoft.Extensions.Logging;
using Pukenicorn.Sukebei.JavList.Client.Api;
using Pukenicorn.Sukebei.JavList.Client.Model;

namespace Jellyfin.Plugin.AvList.Providers.JavList;

public class JavListApi
{
    private readonly ILogger<JavListApi> _logger;
    private readonly IVideosApi _javApiClient;

    public JavListApi(
        ILogger<JavListApi> logger,
        IVideosApi javApiClient)
    {
        _logger = logger;
        _javApiClient = javApiClient;
    }

    public async Task<VideoDto?> GetVideo(string id, CancellationToken cancellationToken)
    {
        var video = await _javApiClient.VideosControllerGetVideoAsync(id, false, cancellationToken)
            .ConfigureAwait(false);

        return video;
    }

// public async Task<Media> GetAnime(string id)
// {
//     return (await WebRequestAPI(AnimeLink.Replace("{0}", id))).data?.Media;
// }
//
// public async Task<RemoteSearchResult> Search_GetSeries(string title, CancellationToken cancellationToken)
// {
//     // Reimplemented instead of calling Search_GetSeries_list() for efficiency
//     RootObject WebContent = await WebRequestAPI(SearchLink.Replace("{0}", title));
//     foreach (MediaSearchResult media in WebContent.data.Page.media)
//     {
//         return media;
//     }
//     return null;
// }

// public async Task<List<MediaSearchResult>> Search_GetSeries_list(string title, CancellationToken cancellationToken)
// {
//     return (await WebRequestAPI(SearchLink.Replace("{0}", title))).data.Page.media;
// }
//
// public async Task<string> FindSeries(string title, CancellationToken cancellationToken)
// {
//     MediaSearchResult result = await Search_GetSeries(title, cancellationToken);
//     if (result != null)
//     {
//         return result.id.ToString();
//     }
//
//     result = await Search_GetSeries(await Equals_check.Clear_name(title, cancellationToken), cancellationToken);
//     if (result != null)
//     {
//         return result.id.ToString();
//     }
//
//     return null;
// }

// /// <summary>
// /// GET and parse JSON content from link, deserialize into a RootObject
// /// </summary>
// /// <param name="link"></param>
// /// <returns></returns>
// public async Task<RootObject> WebRequestAPI(string link)
// {
//     var httpClient = Plugin.Instance!.GetHttpClient();
//     using HttpContent content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>());
//     using var response = await httpClient.PostAsync(link, content).ConfigureAwait(false);
//     using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
//     return await JsonSerializer.DeserializeAsync<RootObject>(responseStream).ConfigureAwait(false);
// }
}
