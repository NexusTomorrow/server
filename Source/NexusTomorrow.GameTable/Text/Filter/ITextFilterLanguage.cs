using NexusTomorrow.Game.Static;
using NexusTomorrow.GameTable.Text.Static;

namespace NexusTomorrow.GameTable.Text.Filter;

public interface ITextFilterLanguage
{
    Language Language { get; }

    /// <summary>
    /// Returns if supplied string meets <see cref="TextFilterClass"/> filtering.
    /// </summary>
    bool IsTextValid(string text, TextFilterClass filterClass);
}
