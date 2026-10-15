namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A union member typed as an interface or abstract class: its arm is the [UnionCase] type whose value the earlier <see cref="Selector"/> member holds.
/// On write the generator sets the selector from the arm, so the two cannot disagree.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class UnionAttribute : Attribute
{
    /// <summary>
    /// Name the selector member.
    /// </summary>
    public UnionAttribute(string selector)
    {
        Selector = selector;
    }

    /// <summary>
    /// The name of the earlier member that selects the arm.
    /// </summary>
    public string Selector { get; }
}
