using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class WarPartyBossToken
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
