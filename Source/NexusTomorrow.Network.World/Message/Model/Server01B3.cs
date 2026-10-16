using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server01B3, Direction.ServerToClient)]
public sealed partial class Server01B3
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }

    [UInt(32)] public uint Unknown6 { get; set; }

    [UInt(32)] public uint Unknown7 { get; set; }
}
