namespace NexusTomorrow.Settings;

/// <summary>
/// Marks a setting whose value is a secret: the host settings page says so, and no failure or log carries the value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class SecretAttribute : Attribute
{
}

/// <summary>
/// The words the host settings page gives a default the options class computes when it is made, such as one that depends on the
/// machine, in place of the value computed where the page is generated.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ComputedDefaultAttribute : Attribute
{
    /// <summary>
    /// Describe the default as <paramref name="words"/>.
    /// </summary>
    public ComputedDefaultAttribute(string words)
    {
        Words = words;
    }

    /// <summary>
    /// The default, in words.
    /// </summary>
    public string Words { get; }
}

/// <summary>
/// The unit a number setting is counted in, which the host settings page writes after its default (<c>25 ms</c>).
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class UnitAttribute : Attribute
{
    /// <summary>
    /// Count the setting in <paramref name="unit"/>.
    /// </summary>
    public UnitAttribute(string unit)
    {
        Unit = unit;
    }

    /// <summary>
    /// The unit, as the page writes it.
    /// </summary>
    public string Unit { get; }
}
