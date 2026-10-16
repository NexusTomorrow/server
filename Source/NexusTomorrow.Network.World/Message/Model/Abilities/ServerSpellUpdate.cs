using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server017B, Direction.ServerToClient)]
public sealed partial class Server017B
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
    [UInt(3)] public byte Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}
