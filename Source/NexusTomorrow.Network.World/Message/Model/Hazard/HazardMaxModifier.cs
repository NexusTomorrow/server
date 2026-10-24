using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// A multiplier and an offset the client applies to a hazard's maximum.
/// </summary>
[WireStruct]
public sealed partial class HazardMaxModifier
{
    /// <summary>
    /// The factor on the maximum. Until the server sends one, the client holds 1.
    /// </summary>
    [VerifiedMember, Float32] public float Multiplier { get; set; }

    /// <summary>
    /// The term added to the maximum. Until the server sends one, the client holds 0.
    /// </summary>
    [VerifiedMember, Float32] public float Offset { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2 { get; set; }
}
