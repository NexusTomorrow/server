using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(3)]
public sealed partial class Server0738Union0Arm03 : Server0738Union0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(5)] public byte Unknown4 { get; set; }
    [UInt(5)] public byte Unknown5 { get; set; }
    [UInt(14)] public ushort Unknown6 { get; set; }
    [UInt(3)] public byte Unknown7 { get; set; }
    [UInt(14)] public ushort Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
    [NarrowString] public string Unknown10 { get; set; }
}
