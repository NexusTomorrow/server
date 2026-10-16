using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Client0828, Direction.ClientToServer)]
public sealed partial class Client0828
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(14)] public ushort Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(14)] public ushort Unknown5 { get; set; }
    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(14)] public ushort Unknown9 { get; set; }
    [UInt(64)] public ulong Unknown10 { get; set; }
    [WideString] public string Unknown11 { get; set; } = "";
}
