using System;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using Newtonsoft.Json;

namespace Jellyfin.Plugin.AvList.Entities;

public class Jav : Video, IHasLookupInfo<JavInfo>
{
    [JsonIgnore]
    public override bool StopRefreshIfLocalMetadataFound => false;

    /// <inheritdoc />
    public override UnratedItem GetBlockUnratedType()
    {
        return UnratedItem.Other;
    }

    public JavInfo GetLookupInfo()
    {
        var info = GetItemLookupInfo<JavInfo>();

        if (IsInMixedFolder)
        {
            return info;
        }

        var name = System.IO.Path.GetFileName(ContainingFolderPath);

        if (VideoType is VideoType.VideoFile or VideoType.Iso)
        {
            if (string.Equals(name, System.IO.Path.GetFileName(Path), StringComparison.OrdinalIgnoreCase))
            {
                // if the folder has the file extension, strip it
                name = System.IO.Path.GetFileNameWithoutExtension(name);
            }
        }

        info.Name = name;

        return info;
    }
}
