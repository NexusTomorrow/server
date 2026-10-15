using NexusTomorrow.Game.Static;

namespace NexusTomorrow.GameTable;

public class GameTableLoader
{
    private readonly List<Type> _gameTables = [];
    private readonly List<Language> _textTables = [];

    public IEnumerable<Type> GetGameTables()
    {
        return _gameTables;
    }

    public IEnumerable<Language> GetTextTables()
    {
        return _textTables;
    }

    public GameTableLoader AddTextTable(Language language)
    {
        _textTables.Add(language);
        return this;
    }

    public GameTableLoader AddGameTable<T>() where T : class
    {
        _gameTables.Add(typeof(T));
        return this;
    }
}
