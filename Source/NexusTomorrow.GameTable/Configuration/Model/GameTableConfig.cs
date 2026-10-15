namespace NexusTomorrow.GameTable.Configuration.Model;

public class GameTableConfig
{
    /// <summary>
    /// Folder holding the extracted client tables. The configuration binder does not enforce <c>required</c>, so <see cref="ServiceCollectionExtensions.AddGameTable"/> rejects a missing or empty value.
    /// </summary>
    public required string GameTablePath { get; set; }
    public CacheConfig Cache { get; set; } = new();
}
