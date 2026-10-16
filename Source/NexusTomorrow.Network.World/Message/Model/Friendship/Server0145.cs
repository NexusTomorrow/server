using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0145, Direction.ServerToClient)]
public sealed partial class Server0145
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
