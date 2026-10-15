using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player successfully starts a Tracking mission.

[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapStarted, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapStarted
{
    [UInt(14)] public ushort PathMissionId { get; set; }
    [UInt(32)] public uint UnitId { get; set; } // Unit that the player is tracking for the mission.
}
