using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it.
/// </summary>
[WireMessage(GameMessageOpcode.Server05CF, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server05CF
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(4)] public byte Unknown0 { get; set; }
}
