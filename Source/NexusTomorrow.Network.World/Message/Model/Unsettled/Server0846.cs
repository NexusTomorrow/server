using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it.
/// </summary>
[WireMessage(GameMessageOpcode.Server0846, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server0846
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
