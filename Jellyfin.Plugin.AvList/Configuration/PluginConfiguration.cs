using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.AvList.Configuration;

public enum TitlePreferenceType
{
    Localized,
    Japanese,
    JapaneseRomanji
}

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        JavListPath = "https://p.pukenicorn.com/javlist";
        TitlePreference = TitlePreferenceType.Localized;
        OriginalTitlePreference = TitlePreferenceType.Japanese;
        CastTitlePreference = TitlePreferenceType.JapaneseRomanji;
    }

    public string JavListPath { get; set; }

    public TitlePreferenceType TitlePreference { get; set; }

    public TitlePreferenceType OriginalTitlePreference { get; set; }

    public TitlePreferenceType CastTitlePreference { get; set; }
}
