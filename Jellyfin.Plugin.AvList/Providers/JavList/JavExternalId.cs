using Jellyfin.Plugin.AvList.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.AvList.Providers.JavList;

public class JavExternalId : IExternalId
{
    public string ProviderName => "JavList";

    public string Key => ProviderNames.JavList;

    public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;

    // TODO correct this
    public string UrlFormatString => "https://anilist.co/anime/{0}/";

    public bool Supports(IHasProviderIds item) => item is Jav;
}
