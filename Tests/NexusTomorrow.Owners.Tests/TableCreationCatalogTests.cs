using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable;
using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.Tests;

public sealed class TableCreationCatalogTests : IDisposable
{
    // CharacterCreation's columns in model order: id, class, race, sex, faction, costume only, sixteen item ids, enabled, start, xp,
    // currency type, currency amount, entitlement.
    private static readonly DataType[] s_creationColumns =
    [
        DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.Boolean,
        .. Enumerable.Repeat(DataType.UInt, 16),
        DataType.Boolean, DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt,
    ];

    // XpPerLevel's columns: id, minimum xp, and four more this catalog does not read.
    private static readonly DataType[] s_levelColumns = [.. Enumerable.Repeat(DataType.UInt, 6)];

    private readonly string _root = Path.Combine(Path.GetTempPath(), $"nt-tables-{Guid.NewGuid():N}");
    private readonly WarningCounter _logger = new();

    public TableCreationCatalogTests()
    {
        Directory.CreateDirectory(Path.Combine(_root, "tbl"));
    }

    private string CreationPath => Path.Combine(_root, "tbl", "CharacterCreation.tbl");

    private string LevelPath => Path.Combine(_root, "tbl", "XpPerLevel.tbl");

    public void Dispose()
    {
        if (!OperatingSystem.IsWindows() && File.Exists(CreationPath))
            File.SetUnixFileMode(CreationPath, UnixFileMode.UserRead | UnixFileMode.UserWrite);
        Directory.Delete(_root, recursive: true);
    }

    [Fact]
    public void AnOptionIsReadWithTheLevelItsXpReaches()
    {
        WriteLevels();
        TableFile.Write(CreationPath, s_creationColumns, Creation(id: 1, faction: 166, xp: 100));

        CreationOption option = Assert.IsType<CreationOption>(Catalog().Find(1));
        Assert.Equal(166u, option.FactionId);
        Assert.Equal(2u, option.Level);
        Assert.True(option.IsOffered);
        Assert.Equal(0, _logger.Warnings);
    }

    [Fact]
    public void ATableThatRepeatsAnIdOffersNothingAndWarns()
    {
        WriteLevels();
        TableFile.Write(CreationPath, s_creationColumns, Creation(id: 1, faction: 166, xp: 0), Creation(id: 1, faction: 167, xp: 0));

        Assert.Null(Catalog().Find(1));
        Assert.Equal(1, _logger.Warnings);
    }

    [Fact]
    public void ATableTheServerCannotOpenOffersNothingAndWarns()
    {
        if (OperatingSystem.IsWindows())
        {
            Assert.Skip("A file's mode is POSIX only.");
            return;
        }

        WriteLevels();
        TableFile.Write(CreationPath, s_creationColumns, Creation(id: 1, faction: 166, xp: 0));
        File.SetUnixFileMode(CreationPath, UnixFileMode.None);
        Assert.SkipWhen(CanOpen(CreationPath), "This user opens a file whatever its mode.");

        Assert.Null(Catalog().Find(1));
        Assert.Equal(1, _logger.Warnings);
    }

    private static bool CanOpen(string path)
    {
        try
        {
            File.OpenRead(path).Dispose();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    // Our own made-up row: a class 1, race 1 option, on, starting on the arkship, with no cost.
    private static uint[] Creation(uint id, uint faction, uint xp) =>
        [id, 1, 1, 0, faction, 0, .. new uint[16], 1, 0, xp, 0, 0, 0];

    // Levels 1, 2 and 3 start at 0, 100 and 300 xp.
    private void WriteLevels() => TableFile.Write(LevelPath, s_levelColumns, [1, 0, 0, 0, 0, 0], [2, 100, 0, 0, 0, 0], [3, 300, 0, 0, 0, 0]);

    private TableCreationCatalog Catalog() => new(Options.Create(new ContentOptions { ClientDataPath = _root }), _logger);

    private sealed class WarningCounter : ILogger<TableCreationCatalog>
    {
        private int _warnings;

        public int Warnings => Volatile.Read(ref _warnings);

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
                Interlocked.Increment(ref _warnings);
        }
    }

    // Writes a table in the layout GameTable reads: the header, one descriptor per column, then the rows, with no strings. Every
    // column here is four bytes wide.
    private static class TableFile
    {
        private const uint Signature = 0x4454424C;
        private const int ColumnDescriptorBytes = 24;

        public static void Write(string path, DataType[] columns, params uint[][] rows)
        {
            ulong recordBytes = (ulong)columns.Length * sizeof(uint);
            using var writer = new BinaryWriter(File.Create(path));
            writer.Write(Signature);
            writer.Write(0u); // version
            writer.Write(0UL); // name length
            writer.Write(0UL); // unknown
            writer.Write(recordBytes);
            writer.Write((ulong)columns.Length);
            writer.Write(0UL); // the descriptors' offset from the end of the header
            writer.Write((ulong)rows.Length);
            writer.Write(recordBytes * (ulong)rows.Length); // every record and no string table
            writer.Write((ulong)(columns.Length * ColumnDescriptorBytes)); // the records' offset from the end of the header
            writer.Write((ulong)rows.Max(row => row[0]) + 1); // one past the highest id
            writer.Write(0UL); // lookup offset
            writer.Write(0UL); // unknown

            foreach (DataType column in columns)
            {
                writer.Write(0UL); // name length
                writer.Write(0UL); // name offset
                writer.Write((ushort)column);
                writer.Write((ushort)0);
                writer.Write(0u);
            }

            foreach (uint[] row in rows)
            {
                foreach (uint value in row)
                    writer.Write(value);
            }
        }
    }
}
