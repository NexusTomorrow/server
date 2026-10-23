namespace NexusTomorrow.Settings;

/// <summary>
/// Marks an options class as one configuration section: its name, its place on the host settings page, and the pages that explain
/// its settings.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class SettingsSectionAttribute : Attribute
{
    /// <summary>
    /// Mark the section <paramref name="name"/>, at <paramref name="order"/> on the host settings page, explained by
    /// <paramref name="explainedBy"/>: each page's title then its target (<c>Page#anchor</c>), in pairs.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="explainedBy"/> is empty or ends with a title that has no target.</exception>
    public SettingsSectionAttribute(string name, int order, params string[] explainedBy)
    {
        if (explainedBy.Length == 0 || explainedBy.Length % 2 != 0)
            throw new ArgumentException("A section names at least one page that explains it, as title and target pairs.", nameof(explainedBy));

        Name = name;
        Order = order;
        ExplainedBy = [.. explainedBy.Chunk(2).Select(pair => new PageLink(pair[0], pair[1]))];
    }

    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The section's place on the host settings page, counted from 1.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// The pages that explain the section's settings, in the order the page names them.
    /// </summary>
    public IReadOnlyList<PageLink> ExplainedBy { get; }
}

/// <summary>
/// A link to a wiki page: the page's title, and its target, the page's name with an optional anchor.
/// </summary>
public sealed record PageLink(string Title, string Target);
