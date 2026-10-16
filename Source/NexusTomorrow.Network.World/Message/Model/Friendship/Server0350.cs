using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0350, Direction.ServerToClient)]
public sealed partial class Server0350
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
