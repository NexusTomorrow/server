using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever a settler interacts with a settler depot.
[WireMessage(GameMessageOpcode.ServerPathSettlerBuildInvoke, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildInvoke
{
    [UInt(32)] public uint SettlerHubUnitId { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> PathSettlerImprovementGroupIds { get; set; } = [];
}
