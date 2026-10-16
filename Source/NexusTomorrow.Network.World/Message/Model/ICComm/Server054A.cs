using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Server054A, Direction.ServerToClient)]
public sealed partial class Server054A
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
