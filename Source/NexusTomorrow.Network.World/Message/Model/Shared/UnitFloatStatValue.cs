using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A stat and the float value that replaces it.
/// </summary>
[WireStruct]
public sealed partial class UnitFloatStatValue
{
    /// <summary>
    /// The stat, 0 to 31. The client keeps stats 1 to 9, 19 and 23 to 25 as floats (9 is dash energy, 2 endurance), and an integer stat sent
    /// here is set to 0.
    /// </summary>
    [UInt(5)] public byte Id { get; set; }

    /// <summary>
    /// The stat's new value, which replaces the old one.
    /// </summary>
    [Float32] public float Value { get; set; }
}
