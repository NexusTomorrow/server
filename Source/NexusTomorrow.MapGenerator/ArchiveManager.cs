using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nexus.Archive;
using NLog;

namespace NexusTomorrow.MapGenerator;

public sealed class ArchiveManager
{
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static readonly string[] localisationIndexes =
    [
        "ClientDataEN.index",
        "ClientDataFR.index",
        "ClientDataDE.index"
    ];

    private Archive? mainArchive;

    /// <summary>
    /// Main client ClientData archive.
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Initialise"/> has not loaded it.</exception>
    public Archive MainArchive => mainArchive ?? throw new InvalidOperationException($"{nameof(MainArchive)} is not loaded; call {nameof(Initialise)} first.");

    /// <summary>
    /// Collection of client localisation archives.
    /// </summary>
    public List<Archive> LocalisationArchives { get; } = [];

    public void Initialise(string patchPath)
    {
        log.Info("Loading archives...");

        // CoreData archive only applicable to Steam client
        ArchiveFile? coreDataArchive = null;
        if (File.Exists(Path.Combine(patchPath, "CoreData.archive")))
            coreDataArchive = ArchiveFileBase.FromFile(Path.Combine(patchPath, "CoreData.archive")) as ArchiveFile;

        mainArchive = Archive.FromFile(Path.Combine(patchPath, "ClientData.index"), coreDataArchive);

        foreach (string localisationArchivePath in localisationIndexes
            .Select(i => Path.Combine(patchPath, i)))
        {
            if (!File.Exists(localisationArchivePath))
                continue;

            LocalisationArchives.Add(Archive.FromFile(localisationArchivePath, coreDataArchive));
        }
    }
}
