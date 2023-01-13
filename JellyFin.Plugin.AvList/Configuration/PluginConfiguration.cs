using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AvList.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            JavListPath = "https://p.pukenicorn.com/javlist";
        }

        public string JavListPath { get; set; }
    }
}
