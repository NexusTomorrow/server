using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathSettlerImprovementBuildTier, Direction.ClientToServer)]
public sealed partial class ClientPathSettlerImprovementBuildTier
{
    [UInt(14)] public uint PathSettlerImprovementGroupId { get; private set; }
    [UInt(32)] public uint BuildTier { get; private set; } // Is zero-based, so 0 = 1st tier, 1 = 2nd tier, etc.
}
