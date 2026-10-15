using System;
using System.IO;
using Nexus.Archive;
using NexusTomorrow.GameTable;
using NexusTomorrow.GameTable.Model;

namespace NexusTomorrow.MapGenerator.GameTable;

public sealed class GameTableManager
{
    private GameTable<WorldEntry>? world;

    /// <summary>
    /// World table from the main client archive.
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Initialise"/> has not loaded it.</exception>
    public GameTable<WorldEntry> World => world ?? throw new InvalidOperationException($"{nameof(World)} is not loaded; call {nameof(Initialise)} first.");

    #region Dependency Injection

    private readonly ArchiveManager archiveManager;

    public GameTableManager(
        ArchiveManager archiveManager)
    {
        this.archiveManager = archiveManager;
    }

    #endregion

    public void Initialise()
    {
        world = LoadGameTable<WorldEntry>("World.tbl");
    }

    /// <summary>
    /// Return <see cref="GameTable{T}"/> for supplied table name found in the main client archive.
    /// </summary>
    private GameTable<T> LoadGameTable<T>(string name) where T : class, new()
    {
        string filePath = Path.Combine("DB", name);
        if (!(archiveManager.MainArchive.IndexFile.FindEntry(filePath) is IArchiveFileEntry file))
            throw new FileNotFoundException();

        using (Stream archiveStream = archiveManager.MainArchive.OpenFileStream(file))
        using (var memoryStream = new MemoryStream())
        {
            archiveStream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return new GameTable<T>(memoryStream);
        }
    }
}
