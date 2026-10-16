using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server034A, Direction.ServerToClient)]
public sealed partial class Server034A
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
