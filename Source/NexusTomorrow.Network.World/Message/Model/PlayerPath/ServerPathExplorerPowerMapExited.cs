using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player leaves a location where a power map mission can be started.

[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapExited, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapExited
{
    [UInt(14)] public ushort PathMissionId { get; set; }
}
