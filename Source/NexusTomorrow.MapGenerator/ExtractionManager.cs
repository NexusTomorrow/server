using System;
using System.IO;
using System.Linq;
using Nexus.Archive;
using NLog;

namespace NexusTomorrow.MapGenerator
{
    public sealed class ExtractionManager
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private static readonly string[] languageFiles =
        {
            "en-US.bin",
            "de-DE.bin",
            "fr-FR.bin",
            "en-GB.bin"
        };

        private string? outputDir;

        #region Dependency Injection

        private readonly ArchiveManager archiveManager;

        public ExtractionManager(
            ArchiveManager archiveManager)
        {
            this.archiveManager = archiveManager;
        }

        #endregion

        public void Initialise(string outputDir)
        {
            log.Info("Extracting GameTables...");

            this.outputDir = Path.Combine(outputDir, "tbl");

            Directory.CreateDirectory(this.outputDir);

            ExtractGameTables();
            ExtractLanguageFiles();
        }

        /// <summary>
        /// Extract all GameTables (*.tbl) from main client archive.
        /// </summary>
        private void ExtractGameTables()
        {
            string searchPattern = Path.Combine("DB", "*.tbl");
            foreach (IArchiveFileEntry fileEntry in archiveManager.MainArchive.IndexFile.GetFiles(searchPattern))
                ExtractFile(archiveManager.MainArchive, fileEntry);
        }

        /// <summary>
        /// Extract all language files (*.bin) from all present localisation client archives.
        /// </summary>
        private void ExtractLanguageFiles()
        {
            foreach (Archive archive in archiveManager.LocalisationArchives)
            {
                foreach (IArchiveFileEntry fileEntry in languageFiles
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
            if (outputDir == null)
                throw new InvalidOperationException($"{nameof(outputDir)} is not set; call {nameof(Initialise)} first.");

            string filePath = Path.Combine(outputDir, fileEntry.FileName);

            using (Stream archiveStream = archive.OpenFileStream(fileEntry))
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                archiveStream.CopyTo(fileStream);
            }

            log.Info($"Extracted {fileEntry.FileName}...");
        }
    }
}
