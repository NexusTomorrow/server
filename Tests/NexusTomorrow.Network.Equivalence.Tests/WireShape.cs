namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// How a wire member sits on the wire, as its attributes declare it.
/// </summary>
internal enum WireShape
{
    Value,
    Array,
    FixedArray,
    Union,
    Blob,
    Pad,
}
