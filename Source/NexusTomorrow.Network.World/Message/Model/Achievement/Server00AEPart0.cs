using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireStruct]
public sealed partial class Server00AEPart0
{

    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
}
