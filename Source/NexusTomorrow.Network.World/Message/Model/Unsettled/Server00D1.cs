using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it.
/// </summary>
[WireMessage(GameMessageOpcode.Server00D1, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server00D1
{
}
