using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0157, Direction.ServerToClient)]
public sealed partial class Server0157
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
