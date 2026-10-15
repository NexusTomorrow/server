using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player fails a Tracking mission.

[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapFailed, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapFailed
{
    [UInt(14)] public ushort PathMissionId { get; set; }
}
