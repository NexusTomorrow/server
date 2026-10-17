using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 0: sets the time of the commands after it.
/// </summary>
[WireStruct]
[UnionCase(0)]
public sealed partial class Server0262Union2Arm00 : IServer0262Union2
{
    /// <summary>
    /// The time, on the sender's clock.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
