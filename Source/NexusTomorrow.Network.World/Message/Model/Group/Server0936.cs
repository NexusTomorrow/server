using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0936, Direction.ServerToClient)]
public sealed partial class Server0936
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
