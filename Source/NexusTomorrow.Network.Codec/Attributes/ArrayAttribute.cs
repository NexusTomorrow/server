namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A counted array member (<c>List&lt;T&gt;</c> or <c>T[]</c>). Set exactly one of <see cref="CountBits"/> (a count written just before the elements) or <see cref="CountFrom"/> (a [Count] member declared earlier).
/// The element is a [WireStruct] type, or a scalar or string described by a second attribute on the member.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ArrayAttribute : Attribute
{
    /// <summary>
    /// The width of the count written immediately before the elements, 1 to 32 bits; 0 when <see cref="CountFrom"/> is set.
    /// </summary>
    public int CountBits { get; set; }

    /// <summary>
    /// The name of an earlier [Count] member that holds the element count; null when <see cref="CountBits"/> is set.
    /// </summary>
    public string? CountFrom { get; set; }

    /// <summary>
    /// The most elements the client holds, or 0 when the count's width is the only bound. Writing more throws; reading is unchanged,
    /// since what the client sends is bounded by the frame.
    /// </summary>
    public int Capacity { get; set; }
}
