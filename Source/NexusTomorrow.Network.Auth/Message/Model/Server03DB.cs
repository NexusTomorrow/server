using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server03DB, Direction.ServerToClient)]
public sealed partial class Server03DB
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [FixedArray(16), UInt(8)] public byte[] Unknown2 { get; set; } = new byte[16];
    [UInt(32)] public uint Unknown3 { get; set; }
    [WideString] public string Unknown4 { get; set; } = "";
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(2)] public byte Unknown6 { get; set; }
    [UInt(21)] public uint Unknown7 { get; set; }
}
