using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03A5, Direction.ServerToClient)]
public sealed partial class Server03A5
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
