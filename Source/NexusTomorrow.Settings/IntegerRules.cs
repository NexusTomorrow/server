namespace NexusTomorrow.Settings;

/// <summary>
/// An integer setting's lowest value.
/// </summary>
public sealed class AtLeastAttribute : SettingRuleAttribute
{
    /// <summary>
    /// Require at least <paramref name="minimum"/>.
    /// </summary>
    public AtLeastAttribute(long minimum)
    {
        Minimum = minimum;
    }

    /// <summary>
    /// The lowest value allowed.
    /// </summary>
    public long Minimum { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting) => $"At least {SettingText.Prose(Minimum)}";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        SettingText.Integer(value) >= Minimum ? null : $"{setting.Key} must be at least {SettingText.Digits(Minimum)}.";
}

/// <summary>
/// An integer setting that may be zero but never below it.
/// </summary>
public sealed class NotNegativeAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "Not negative";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        SettingText.Integer(value) >= 0 ? null : $"{setting.Key} must not be negative.";
}

/// <summary>
/// An integer setting that takes one of a fixed set of values.
/// </summary>
public sealed class OneOfAttribute : SettingRuleAttribute
{
    /// <summary>
    /// Allow only <paramref name="values"/>, which the page and the failure list in this order.
    /// </summary>
    public OneOfAttribute(params long[] values)
    {
        Values = [.. values];
    }

    /// <summary>
    /// The values allowed.
    /// </summary>
    public IReadOnlyList<long> Values { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting) => $"One of {SettingText.Alternatives(Values.Select(SettingText.Prose))}";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        Values.Contains(SettingText.Integer(value)) ? null : $"{setting.Key} must be {SettingText.Alternatives(Values.Select(SettingText.Digits))}.";
}

/// <summary>
/// An integer setting's lowest and highest values, both allowed.
/// </summary>
public sealed class BetweenAttribute : SettingRuleAttribute
{
    /// <summary>
    /// Require <paramref name="minimum"/> to <paramref name="maximum"/>.
    /// </summary>
    public BetweenAttribute(long minimum, long maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    /// <summary>
    /// The lowest value allowed.
    /// </summary>
    public long Minimum { get; }

    /// <summary>
    /// The highest value allowed.
    /// </summary>
    public long Maximum { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting) => $"{SettingText.Prose(Minimum)} to {SettingText.Prose(Maximum)}";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value)
    {
        long number = SettingText.Integer(value);
        return number >= Minimum && number <= Maximum ? null : $"{setting.Key} must be {SettingText.Digits(Minimum)} to {SettingText.Digits(Maximum)}.";
    }
}
