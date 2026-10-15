namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Marks a [WireStruct] type as the union arm for one selector value.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class UnionCaseAttribute : Attribute
{
    /// <summary>
    /// Declare the selector value, an enum value or an integer.
    /// </summary>
    public UnionCaseAttribute(object value)
    {
        Value = value;
    }

    /// <summary>
    /// The selector value.
    /// </summary>
    public object Value { get; }
}
