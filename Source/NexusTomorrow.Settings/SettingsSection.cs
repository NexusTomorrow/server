using System.Collections;
using System.Globalization;
using System.Reflection;

namespace NexusTomorrow.Settings;

/// <summary>
/// An options class read as one configuration section: its settings in declaration order, each with its key, its default and its
/// rules, and the rules that span several of them. The host's check at start and the host settings page both read it, so the two
/// state the same limits.
/// </summary>
/// <remarks>
/// A property whose type is a class of settings (a listener) is not a setting of its own: its properties are, keyed through it
/// (<c>Gateway:Sts:Port</c>). A list is one setting, and when its elements are classes of settings each of their properties follows
/// it, keyed <c>Realm:StartLocations:&lt;n&gt;:FactionId</c>, with the default a new element holds.
/// </remarks>
public sealed class SettingsSection
{
    private readonly Dictionary<string, Setting> _byKey;

    private SettingsSection(Type type, SettingsSectionAttribute section, IReadOnlyList<Setting> settings)
    {
        Type = type;
        Name = section.Name;
        Order = section.Order;
        ExplainedBy = section.ExplainedBy;
        Settings = settings;
        CrossRules = [.. type.GetCustomAttributes<CrossSettingRuleAttribute>(inherit: false)];
        _byKey = settings.ToDictionary(setting => setting.Key, StringComparer.Ordinal);
    }

    /// <summary>
    /// The options class.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The section's place on the host settings page, counted from 1.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// The pages that explain the section's settings.
    /// </summary>
    public IReadOnlyList<PageLink> ExplainedBy { get; }

    /// <summary>
    /// The section's settings, in declaration order.
    /// </summary>
    public IReadOnlyList<Setting> Settings { get; }

    /// <summary>
    /// The rules that span several settings, in declaration order.
    /// </summary>
    public IReadOnlyList<CrossSettingRuleAttribute> CrossRules { get; }

    /// <summary>
    /// Read <paramref name="type"/>, an options class marked with <see cref="SettingsSectionAttribute"/>; its defaults are those of a
    /// new instance.
    /// </summary>
    /// <exception cref="InvalidOperationException"><paramref name="type"/> has no section attribute, or a rule sits where it is never
    /// checked: on a class of settings, or inside a list.</exception>
    public static SettingsSection Of(Type type)
    {
        SettingsSectionAttribute section = type.GetCustomAttribute<SettingsSectionAttribute>()
            ?? throw new InvalidOperationException($"{type.FullName} is not marked as a settings section.");
        var settings = new List<Setting>();
        Add(settings, section.Name, type, New(type), [], inList: false);
        return new SettingsSection(type, section, settings);
    }

    /// <summary>
    /// The full key of a setting named within this section, such as <c>Gateway:Sts:Port</c> for <c>Sts:Port</c>.
    /// </summary>
    public string Key(string relative) => $"{Name}:{relative}";

    /// <summary>
    /// The setting named <paramref name="relative"/> within this section.
    /// </summary>
    /// <exception cref="InvalidOperationException">The section has no such setting.</exception>
    public Setting Find(string relative) =>
        _byKey.TryGetValue(Key(relative), out Setting? setting) ? setting : throw new InvalidOperationException($"{Key(relative)} is not a setting.");

    /// <summary>
    /// Every failure of <paramref name="options"/> against the section's rules: each setting's rules in declaration order, then the
    /// rules that span several. Each names keys and never values; an empty list means every rule passes.
    /// </summary>
    public IReadOnlyList<string> Check(object options)
    {
        var failures = new List<string>();
        foreach (Setting setting in Settings.Where(setting => setting.Rules.Count > 0))
        {
            object? value = setting.ValueIn(options);
            foreach (SettingRuleAttribute rule in setting.Rules)
            {
                if (rule.Check(setting, value) is { } failure)
                    failures.Add(failure);
            }
        }

        foreach (CrossSettingRuleAttribute rule in CrossRules)
        {
            if (rule.Check(this, options) is { } failure)
                failures.Add(failure);
        }

        return failures;
    }

    private static void Add(List<Setting> settings, string prefix, Type type, object? instance, PropertyInfo[] path, bool inList)
    {
        foreach (PropertyInfo property in Bindable(type))
        {
            string key = $"{prefix}:{property.Name}";
            object? value = instance is null ? null : property.GetValue(instance);
            PropertyInfo[] here = [.. path, property];
            Type? element = ElementType(property.PropertyType);
            if (IsGroup(property.PropertyType))
            {
                if (property.IsDefined(typeof(SettingRuleAttribute)))
                    throw new InvalidOperationException($"{key} holds settings of its own, so a rule on it is never checked.");
                Add(settings, key, property.PropertyType, value, here, inList);
                continue;
            }

            if (inList && property.IsDefined(typeof(SettingRuleAttribute)))
                throw new InvalidOperationException($"{key} sits inside a list, so a rule on it is never checked.");
            settings.Add(new Setting(key, property, value, inList ? null : here));
            if (element is not null && IsGroup(element))
                Add(settings, $"{key}:<n>", element, New(element), [], inList: true);
        }
    }

    // What the configuration binder fills: public instance properties it can read and set, in declaration order.
    private static IEnumerable<PropertyInfo> Bindable(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.GetMethod is { IsPublic: true } && property.SetMethod is { IsPublic: true } && property.GetIndexParameters().Length == 0)
            .OrderBy(property => property.MetadataToken);

    private static bool IsGroup(Type type) => type.IsClass && type != typeof(string) && ElementType(type) is null;

    private static Type? ElementType(Type type) =>
        type == typeof(string)
            ? null
            : type.GetInterfaces().Append(type).FirstOrDefault(face => face.IsGenericType && face.GetGenericTypeDefinition() == typeof(IEnumerable<>))?.GetGenericArguments()[0];

    // Options classes are made by the binder through their parameterless constructor, so their defaults are a new instance's.
    private static object New(Type type) =>
        Activator.CreateInstance(type) ?? throw new InvalidOperationException($"{type.FullName} cannot be made without arguments.");
}

/// <summary>
/// One setting of a <see cref="SettingsSection"/>: its key, its default, its rules and how the host settings page describes it.
/// </summary>
public sealed class Setting
{
    private const string None = "none";

    private readonly PropertyInfo[]? _path;

    internal Setting(string key, PropertyInfo property, object? defaultValue, PropertyInfo[]? path)
    {
        Key = key;
        Property = property;
        Default = defaultValue;
        _path = path;
        Rules = [.. property.GetCustomAttributes<SettingRuleAttribute>(inherit: false)];
        IsSecret = property.IsDefined(typeof(SecretAttribute));
        ComputedDefault = property.GetCustomAttribute<ComputedDefaultAttribute>()?.Words;
        Unit = property.GetCustomAttribute<UnitAttribute>()?.Unit;
    }

    /// <summary>
    /// The configuration key, its parts joined by colons (<c>Maps:TickMs</c>).
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// The property that holds the setting.
    /// </summary>
    public PropertyInfo Property { get; }

    /// <summary>
    /// The value the setting holds when the configuration does not set it.
    /// </summary>
    public object? Default { get; }

    /// <summary>
    /// The setting's rules.
    /// </summary>
    public IReadOnlyList<SettingRuleAttribute> Rules { get; }

    /// <summary>
    /// Whether the setting's value is a secret.
    /// </summary>
    public bool IsSecret { get; }

    /// <summary>
    /// The words for a default the options class computes, or null.
    /// </summary>
    public string? ComputedDefault { get; }

    /// <summary>
    /// The unit a number setting is counted in, or null.
    /// </summary>
    public string? Unit { get; }

    /// <summary>
    /// The default as the configuration would hold it, in plain digits: a duration in whole seconds as <c>60 s</c> and finer as
    /// <c>25 ms</c>, a switch as <c>on</c> or <c>off</c>, a number with its <see cref="Unit"/>, the words of a
    /// <see cref="ComputedDefault"/>, and <c>none</c> for an empty or unset value.
    /// </summary>
    public string DefaultText => ComputedDefault ?? Default switch
    {
        null or "" => None,
        string text => text,
        bool on => on ? "on" : "off",
        TimeSpan duration => $"{SettingText.Amount(duration, SettingText.DurationUnit(duration), separators: false)} {SettingText.DurationUnit(duration)}",
        IEnumerable values => values.Cast<object>().Select(value => Convert.ToString(value, CultureInfo.InvariantCulture)).ToArray() is { Length: > 0 } items ? string.Join(", ", items) : None,
        IFormattable number => Unit is null ? number.ToString(null, CultureInfo.InvariantCulture) : $"{number.ToString(null, CultureInfo.InvariantCulture)} {Unit}",
        var other => other.ToString() ?? None,
    };

    /// <summary>
    /// The setting's rules as the page's Must be cell states them, joined by semicolons; empty when it has none.
    /// </summary>
    public string MustBe => string.Join("; ", Rules.Select(rule => rule.MustBe(this)));

    /// <summary>
    /// The setting's value in <paramref name="options"/>, an instance of its section's options class.
    /// </summary>
    /// <exception cref="InvalidOperationException">The setting is a property of a list's elements, which holds one value per element.</exception>
    public object? ValueIn(object options)
    {
        if (_path is null)
            throw new InvalidOperationException($"{Key} is a property of a list's elements, so it holds no single value.");

        object? value = options;
        foreach (PropertyInfo step in _path)
            value = value is null ? null : step.GetValue(value);
        return value;
    }
}
