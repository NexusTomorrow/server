using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildHolomark
{
    [WireStruct]
    public sealed partial class Part
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(14)] public ushort Unknown1 { get; set; }
        [UInt(14)] public ushort Unknown2 { get; set; }
        [UInt(14)] public ushort Unknown3 { get; set; }
    }

    public Part Unknown0 { get; set; } = new();
    public Part Unknown1 { get; set; } = new();
    public Part Unknown2 { get; set; } = new();
}
