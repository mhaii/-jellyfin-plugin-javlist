using Jellyfin.Plugin.AvList.Providers.JavList;
using MediaBrowser.Common.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pukenicorn.Sukebei.JavList.Client.Api;

namespace Jellyfin.Plugin.AvList;

public class PluginServiceRegistrator : IPluginServiceRegistrator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IVideosApi>(sp
            => new VideosApi(Plugin.Instance!.GetHttpClient()));
        serviceCollection.AddSingleton<JavListApi>(sp
            => new JavListApi(sp.GetService<ILogger<JavListApi>>()!, sp.GetService<IVideosApi>()!));
    }
}
