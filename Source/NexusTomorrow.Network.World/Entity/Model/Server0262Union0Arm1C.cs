using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 28. The client refuses a unit of this type.
/// </summary>
[WireStruct]
[UnionCase(28)]
public sealed partial class Server0262Union0Arm1C : IServer0262Union0
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown0 { get; set; }
}
