using System;
using System.IO;
using System.Linq;
using Nexus.Archive;
using NLog;

namespace NexusTomorrow.MapGenerator;

public sealed class ExtractionManager
{
    private static readonly ILogger s_log = LogManager.GetCurrentClassLogger();

    private static readonly string[] s_languageFiles =
    [
        "en-US.bin",
        "de-DE.bin",
        "fr-FR.bin",
        "en-GB.bin"
    ];

    private string? _outputDir;

    private readonly ArchiveManager _archiveManager;

    public ExtractionManager(
        ArchiveManager archiveManager)
    {
        _archiveManager = archiveManager;
    }

    public void Initialize(string outputDir)
    {
        s_log.Info("Extracting GameTables...");

        _outputDir = Path.Combine(outputDir, "tbl");

        Directory.CreateDirectory(_outputDir);

        ExtractGameTables();
        ExtractLanguageFiles();
    }

    /// <summary>
    /// Extract all GameTables (*.tbl) from main client archive.
    /// </summary>
    private void ExtractGameTables()
    {
        string searchPattern = Path.Combine("DB", "*.tbl");
        foreach (IArchiveFileEntry fileEntry in _archiveManager.MainArchive.IndexFile.GetFiles(searchPattern))
            ExtractFile(_archiveManager.MainArchive, fileEntry);
    }

    /// <summary>
    /// Extract all language files (*.bin) from all present localisation client archives.
    /// </summary>
    private void ExtractLanguageFiles()
    {
        foreach (Archive archive in _archiveManager.LocalizationArchives)
        {
            foreach (IArchiveFileEntry fileEntry in s_languageFiles
                .Select(archive.IndexFile.FindEntry)
                .OfType<IArchiveFileEntry>())
            {
                ExtractFile(archive, fileEntry);
            }
        }
    }

    /// <summary>
    /// Extract supplied <see cref="IArchiveFileEntry"/> from <see cref="Archive"/>.
    /// </summary>
    private void ExtractFile(Archive archive, IArchiveFileEntry fileEntry)
    {
        if (_outputDir == null)
            throw new InvalidOperationException($"{nameof(_outputDir)} is not set; call {nameof(Initialize)} first.");

        string filePath = Path.Combine(_outputDir, fileEntry.FileName);

        using (Stream archiveStream = archive.OpenFileStream(fileEntry))
        using (FileStream fileStream = File.OpenWrite(filePath))
        {
            archiveStream.CopyTo(fileStream);
        }

        s_log.Info($"Extracted {fileEntry.FileName}...");
    }
}
