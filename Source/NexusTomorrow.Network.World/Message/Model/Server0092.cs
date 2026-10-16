using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0092, Direction.ServerToClient)]
public sealed partial class Server0092
{
    [WireStruct]
    public sealed partial class Server0092Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [FixedArray(15), UInt(64)] public ulong[] Unknown2 { get; set; }
        [FixedArray(19), UInt(64)] public ulong[] Unknown3 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    public Server0092Part0 Unknown2 { get; set; }
}
