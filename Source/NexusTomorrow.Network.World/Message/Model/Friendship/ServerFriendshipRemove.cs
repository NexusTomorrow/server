using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C3, Direction.ServerToClient)]
public sealed partial class Server03C3
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
