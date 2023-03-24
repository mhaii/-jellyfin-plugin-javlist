using System;
using System.Collections.Generic;
using System.Linq;
using Jellyfin.Plugin.AvList.Configuration;
using Jellyfin.Plugin.AvList.Entities;
using Jellyfin.Plugin.AvList.Providers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Pukenicorn.Sukebei.JavList.Client.Model;

namespace Jellyfin.Plugin.AvList.Extensions;

public static class VideoDtoExtensions
{
    public static string GetPreferredTitle(this VideoDto video, TitlePreferenceType preference, string language)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        switch (preference)
        {
            case TitlePreferenceType.Localized:
                switch (language)
                {
                    case "en":
                        return video.Name;
                    case "jap":
                        return video.Name;
                }

                break;
            case TitlePreferenceType.Japanese:
                return video.Name;
            case TitlePreferenceType.JapaneseRomanji:
            default:
                return video.Name;
        }

        return string.Empty;
    }

    public static IEnumerable<string> GetPreferredActorNames(this VideoDto video, TitlePreferenceType preference, string language)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        return PrivateGetPreferredPeopleNames(video.Actors, preference, language);
    }

    public static IEnumerable<string> GetPreferredDirectorNames(this VideoDto video, TitlePreferenceType preference, string language)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        return PrivateGetPreferredPeopleNames(video.Directors, preference, language);
    }

    private static IEnumerable<string> PrivateGetPreferredPeopleNames(IEnumerable<string> list, TitlePreferenceType preference, string language)
    {
        return list.Select(name =>
        {
            switch (preference)
            {
                case TitlePreferenceType.Localized:
                    switch (language)
                    {
                        case "en":
                            return name;
                        case "jap":
                            return name;
                    }

                    break;
                case TitlePreferenceType.Japanese:
                    return name;
                case TitlePreferenceType.JapaneseRomanji:
                default:
                    return name;
            }

            return string.Empty;
        });
    }

    public static RemoteSearchResult ToSearchResult(this VideoDto video)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        PluginConfiguration config = Plugin.Instance!.Configuration;

        return new RemoteSearchResult
        {
            Name = video.GetPreferredTitle(config.TitlePreference, "en"),
            ProductionYear = video.ReleaseDate?.Year,
            PremiereDate = video.ReleaseDate,
            ImageUrl = video.CoverUrl,
            SearchProviderName = ProviderNames.JavList,
            ProviderIds = new Dictionary<string, string> { { ProviderNames.JavList, video.Code } }
        };
    }

    public static Jav ToJav(this VideoDto video)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        PluginConfiguration config = Plugin.Instance!.Configuration;

        return new Jav
        {
            Name = video.GetPreferredTitle(config.TitlePreference, "en"),
            OriginalTitle = video.GetPreferredTitle(config.OriginalTitlePreference, "en"),
            ProductionYear = video.ReleaseDate?.Year,
            PremiereDate = video.ReleaseDate,
            EndDate = video.ReleaseDate,
            Tags = video.Tags.ToArray(),
            Studios = new[] { video.Label },
            ProviderIds = new Dictionary<string, string> { { ProviderNames.JavList, video.Code } }
        };
    }

    public static IEnumerable<PersonInfo> GetCasts(this VideoDto video)
    {
        if (video == null)
        {
            throw new ArgumentNullException(nameof(video));
        }

        PluginConfiguration config = Plugin.Instance!.Configuration;
        List<PersonInfo> lpi = new List<PersonInfo>();

        foreach (string name in video.GetPreferredActorNames(config.CastTitlePreference, "en"))
        {
            PeopleHelper.AddPerson(lpi, new PersonInfo
            {
                Name = name,
                // ImageUrl = va.image.large ?? va.image.medium,
                Role = "Self",
                Type = PersonType.Actor,
                ProviderIds = new Dictionary<string, string> { { ProviderNames.JavList, video.Code } }
            });
        }

        foreach (string name in video.GetPreferredDirectorNames(config.CastTitlePreference, "en"))
        {
            PeopleHelper.AddPerson(lpi, new PersonInfo
            {
                Name = name,
                // ImageUrl = va.image.large ?? va.image.medium,
                Role = "Self",
                Type = PersonType.Director,
                ProviderIds = new Dictionary<string, string> { { ProviderNames.JavList, video.Code } }
            });
        }

        return lpi;
    }
}
