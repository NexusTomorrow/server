using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server042F, Direction.ServerToClient)]
public sealed partial class Server042F
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(3)] public byte Unknown2 { get; set; }

    [UInt(3)] public byte Unknown3 { get; set; }

    [UInt(4)] public byte Unknown4 { get; set; }

    [UInt(2)] public byte Unknown5 { get; set; }
}
