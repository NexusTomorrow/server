using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Client0481Part0
{
    [WireStruct]
    public sealed partial class Client0481Part1
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(14)] public ushort Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(14)] public ushort Unknown3 { get; set; }
    }

    public Client0481Part1 Unknown0 { get; set; } = new();
    public Client0481Part1 Unknown1 { get; set; } = new();
    public Client0481Part1 Unknown2 { get; set; } = new();
}
