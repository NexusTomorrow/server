using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server088C, Direction.ServerToClient)]
public sealed partial class Server088C
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(5)] public byte Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
