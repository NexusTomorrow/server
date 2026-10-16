using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server034F, Direction.ServerToClient)]
public sealed partial class Server034F
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
}
