using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client042E, Direction.ClientToServer)]
public sealed partial class Client042E
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(3)] public byte Unknown1 { get; set; }

    [UInt(3)] public byte Unknown2 { get; set; }

    [UInt(4)] public byte Unknown3 { get; set; }

    [UInt(2)] public byte Unknown4 { get; set; }
}
