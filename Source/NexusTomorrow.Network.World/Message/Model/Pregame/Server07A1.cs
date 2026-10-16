using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server07A1, Direction.ServerToClient)]
public sealed partial class Server07A1
{
    [WireStruct]
    public sealed partial class Server07A1Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [FixedArray(16), UInt(8)] public byte[] Unknown1 { get; set; } = new byte[16];
    public Server07A1Part0 Unknown2 { get; set; } = new();
    [UInt(1)] public bool Unknown3 { get; set; }
    [WideString] public string Unknown4 { get; set; } = "";
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(2)] public byte Unknown6 { get; set; }
    [UInt(21)] public uint Unknown7 { get; set; }
}
