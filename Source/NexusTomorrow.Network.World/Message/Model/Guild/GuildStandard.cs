using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildStandard
{
    [WireStruct]
    public sealed partial class GuildStandardPart
    {
        [UInt(14)] public ushort GuildStandardPartId { get; set; }
        [UInt(14)] public ushort DyeColorRampId1 { get; set; }
        [UInt(14)] public ushort DyeColorRampId2 { get; set; }
        [UInt(14)] public ushort DyeColorRampId3 { get; set; }
    }

    public GuildStandardPart BackgroundIcon { get; set; } = new();
    public GuildStandardPart ForegroundIcon { get; set; } = new();
    public GuildStandardPart ScanLines { get; set; } = new();
}
