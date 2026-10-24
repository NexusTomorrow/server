using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 21. The client refuses a unit of this type.
/// </summary>
[WireStruct]
[UnionCase(21)]
public sealed partial class Server0262Union0Arm15 : IServer0262Union0
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint Unknown0 { get; set; }
}
