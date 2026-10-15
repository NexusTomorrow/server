using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nexus.Archive;
using NLog;

namespace NexusTomorrow.MapGenerator;

public sealed class ArchiveManager
{
    private static readonly Logger s_log = LogManager.GetCurrentClassLogger();

    private static readonly string[] s_localizationIndexes =
    [
        "ClientDataEN.index",
        "ClientDataFR.index",
        "ClientDataDE.index"
    ];

    private Archive? _mainArchive;

    /// <summary>
    /// Main client ClientData archive.
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Initialize"/> has not loaded it.</exception>
    public Archive MainArchive => _mainArchive ?? throw new InvalidOperationException($"{nameof(MainArchive)} is not loaded; call {nameof(Initialize)} first.");

    /// <summary>
    /// Collection of client localisation archives.
    /// </summary>
    public List<Archive> LocalizationArchives { get; } = [];

    public void Initialize(string patchPath)
    {
        s_log.Info("Loading archives...");

        // CoreData archive only applicable to Steam client
        ArchiveFile? coreDataArchive = null;
        if (File.Exists(Path.Combine(patchPath, "CoreData.archive")))
            coreDataArchive = ArchiveFileBase.FromFile(Path.Combine(patchPath, "CoreData.archive")) as ArchiveFile;

        _mainArchive = Archive.FromFile(Path.Combine(patchPath, "ClientData.index"), coreDataArchive);

        foreach (string localisationArchivePath in s_localizationIndexes
            .Select(i => Path.Combine(patchPath, i)))
        {
            if (!File.Exists(localisationArchivePath))
                continue;

            LocalizationArchives.Add(Archive.FromFile(localisationArchivePath, coreDataArchive));
        }
    }
}
