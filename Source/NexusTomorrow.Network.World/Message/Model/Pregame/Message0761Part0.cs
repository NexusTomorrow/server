using NexusTomorrow.Game.Static.Pregame;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireStruct]
public partial class Message0761Part0
{
    [WireStruct]
    public sealed partial class Message0761Part1
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(2)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
    [UInt(3)] public byte Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [FixedArray(16), UInt(8)] public byte[] Unknown8 { get; set; } = new byte[16];
    public Message0761Part1 Unknown9 { get; set; }
    [UInt(16)] public ushort Unknown10 { get; set; }
    [UInt(16)] public ushort Unknown11 { get; set; }
    [UInt(16)] public ushort Unknown12 { get; set; }
    [UInt(16)] public ushort Unknown13 { get; set; }
}
