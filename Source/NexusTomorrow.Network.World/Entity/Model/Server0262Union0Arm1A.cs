using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 26. The client refuses a unit of this type.
/// </summary>
[WireStruct]
[UnionCase(26)]
public sealed partial class Server0262Union0Arm1A : IServer0262Union0
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown0 { get; set; }
}
