using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it.
/// </summary>
[WireMessage(GameMessageOpcode.Server06DE, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server06DE
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown0 { get; set; }
}
