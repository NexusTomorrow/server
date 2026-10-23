namespace NexusTomorrow.Settings;

/// <summary>
/// Settings of one section that must all differ, such as ports that cannot be shared.
/// </summary>
public sealed class DistinctSettingsAttribute : CrossSettingRuleAttribute
{
    /// <summary>
    /// Require the settings at <paramref name="settings"/> (keys within the section, such as <c>Sts:Port</c>) to differ. The page's
    /// sentence calls them <paramref name="noun"/>, and the failure <paramref name="described"/>, such as "the STS, login and world
    /// ports".
    /// </summary>
    public DistinctSettingsAttribute(string noun, string described, params string[] settings)
    {
        Noun = noun;
        Described = described;
        Settings = [.. settings];
    }

    /// <summary>
    /// What the settings are, in the plural.
    /// </summary>
    public string Noun { get; }

    /// <summary>
    /// The settings as the failure names them.
    /// </summary>
    public string Described { get; }

    /// <summary>
    /// The settings' keys within the section.
    /// </summary>
    public IReadOnlyList<string> Settings { get; }

    /// <inheritdoc/>
    public override string Sentence(SettingsSection section) =>
        $"{SettingText.List(Settings.Select(setting => SettingText.Code(section.Key(setting))))} must be {SettingText.Count(Settings.Count)} different {Noun}.";

    /// <inheritdoc/>
    public override string? Check(SettingsSection section, object options)
    {
        object?[] values = [.. Settings.Select(setting => section.Find(setting).ValueIn(options))];
        return values.Distinct().Count() == values.Length ? null : $"{section.Name}: {Described} must be {SettingText.Count(Settings.Count)} different {Noun}.";
    }
}

/// <summary>
/// Two integer settings of one section whose product must fall within a range, both ends allowed. The page's sentence names the
/// factors in the order given; the failure names the second first.
/// </summary>
public sealed class ProductBetweenAttribute : CrossSettingRuleAttribute
{
    /// <summary>
    /// Require <paramref name="first"/> times <paramref name="second"/> (keys within the section) to be <paramref name="minimum"/> to
    /// <paramref name="maximum"/>, counted in <paramref name="unit"/>.
    /// </summary>
    public ProductBetweenAttribute(string first, string second, long minimum, long maximum, string unit)
    {
        First = first;
        Second = second;
        Minimum = minimum;
        Maximum = maximum;
        Unit = unit;
    }

    /// <summary>
    /// The first factor's key within the section.
    /// </summary>
    public string First { get; }

    /// <summary>
    /// The second factor's key within the section.
    /// </summary>
    public string Second { get; }

    /// <summary>
    /// The lowest product allowed.
    /// </summary>
    public long Minimum { get; }

    /// <summary>
    /// The highest product allowed.
    /// </summary>
    public long Maximum { get; }

    /// <summary>
    /// The unit the product is counted in.
    /// </summary>
    public string Unit { get; }

    /// <inheritdoc/>
    public override string Sentence(SettingsSection section) =>
        $"{SettingText.Code(section.Key(First))} times {SettingText.Code(section.Key(Second))} must be from {SettingText.Prose(Minimum)} to {SettingText.Prose(Maximum)} {Unit}.";

    /// <inheritdoc/>
    public override string? Check(SettingsSection section, object options)
    {
        long product = SettingText.Integer(section.Find(First).ValueIn(options)) * SettingText.Integer(section.Find(Second).ValueIn(options));
        return product >= Minimum && product <= Maximum
            ? null
            : $"{section.Key(Second)} times {section.Key(First)} must be {SettingText.Digits(Minimum)} to {SettingText.Digits(Maximum)} {Unit}.";
    }
}

/// <summary>
/// A setting of one section that must not exceed another of the same type, such as a first delay and the longest delay.
/// </summary>
public sealed class NotAboveAttribute : CrossSettingRuleAttribute
{
    /// <summary>
    /// Require <paramref name="bounded"/> to be at most <paramref name="limit"/> (keys within the section).
    /// </summary>
    public NotAboveAttribute(string bounded, string limit)
    {
        Bounded = bounded;
        Limit = limit;
    }

    /// <summary>
    /// The bounded setting's key within the section.
    /// </summary>
    public string Bounded { get; }

    /// <summary>
    /// The bounding setting's key within the section.
    /// </summary>
    public string Limit { get; }

    /// <inheritdoc/>
    public override string Sentence(SettingsSection section) =>
        $"{SettingText.Code(section.Key(Bounded))} must not exceed {SettingText.Code(section.Key(Limit))}.";

    /// <inheritdoc/>
    public override string? Check(SettingsSection section, object options) =>
        Comparer<object>.Default.Compare(section.Find(Bounded).ValueIn(options), section.Find(Limit).ValueIn(options)) > 0
            ? $"{section.Key(Bounded)} must not exceed {section.Key(Limit)}."
            : null;
}
