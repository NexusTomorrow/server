using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireStruct]
public sealed partial class SettlerImprovementGroupStatus
{
    [UInt(14)] public ushort PathSettlerImprovementGroupId { get; set; }
    [UInt(32)] public int Tier { get; set; } // -1 seems to mean the improvement is inactive
    [UInt(32)] public uint RemainingTimeMS { get; set; }
    [UInt(32)] public uint BundleCount { get; set; }
}
