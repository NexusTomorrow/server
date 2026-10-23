namespace NexusTomorrow.Settings;

/// <summary>
/// A rule on one setting, declared on its property: the text the host settings page shows in its Must be column, and the check the
/// host runs at start. A failure names the setting's key and never its value, so a secret or a connection string never reaches a log.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public abstract class SettingRuleAttribute : Attribute
{
    /// <summary>
    /// The rule as the page's Must be cell states it, numbers written with a thousands separator from 1,000 up.
    /// </summary>
    public abstract string MustBe(Setting setting);

    /// <summary>
    /// The failure for <paramref name="value"/>, naming <paramref name="setting"/>'s key and never the value, its numbers in plain
    /// digits; null when the value passes.
    /// </summary>
    public abstract string? Check(Setting setting, object? value);
}

/// <summary>
/// A rule over several settings of one section, declared on its options class: the sentence the host settings page writes under the
/// section's table, and the check the host runs at start. A failure names keys and never values.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public abstract class CrossSettingRuleAttribute : Attribute
{
    /// <summary>
    /// The rule as one sentence of the page, each setting as a code span, numbers written with a thousands separator from 1,000 up.
    /// </summary>
    public abstract string Sentence(SettingsSection section);

    /// <summary>
    /// The failure for <paramref name="options"/>, naming keys and never values, its numbers in plain digits; null when they pass.
    /// </summary>
    public abstract string? Check(SettingsSection section, object options);
}
